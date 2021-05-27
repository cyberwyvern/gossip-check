import pickle

import numpy as np
import tensorflow as tf
from flask import Flask, json, request
from flask_cors import CORS

from ai.util import FNCData, pipeline_test

app = Flask(__name__)
cors = CORS(app, resources={r"/*": {"origins": "*"}})

model_checkpoint_path = "checkpoint/model.h5"
vectorizer_checkpoint_path = "checkpoint/vectorizer.dump"

dump_file = open(vectorizer_checkpoint_path, 'rb')
(bow_vectorizer, tfreq_vectorizer, tfidf_vectorizer) = pickle.load(dump_file)

model = tf.keras.models.load_model(model_checkpoint_path)


@app.route("/predict", methods=['POST'])
def predict():
    input = json.loads(request.data)

    bodies = []
    headlines = []
    for index, (_, value) in enumerate(input['bodies'].items()):
        bodies.append({'BodyID': index, 'text': value})
        headlines.append({'BodyID': index, 'Headlines': input['headline']})

    raw_data = FNCData(headlines, bodies)

    data = pipeline_test(raw_data,
                         bow_vectorizer,
                         tfreq_vectorizer,
                         tfidf_vectorizer)

    prediction = model.predict(np.array(data))

    result = {}
    for index, (key, _) in enumerate(input['bodies'].items()):
        stance = np.argmax(prediction[index])
        stance = {
            0: 'agree',
            1: 'disagree',
            2: 'discuss',
            3: 'unrelated'
        }[stance]

        result[key] = stance

    return json.dumps(result)


if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)
