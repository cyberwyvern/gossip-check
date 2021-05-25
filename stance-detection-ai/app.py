import pickle

import numpy as np
import tensorflow as tf
from flask import Flask, json, request
from flask_cors import CORS

from ai.util import FNCData, pipeline_test

app = Flask(__name__)
cors = CORS(app, resources={r"/*": {"origins": "*"}})

model_checkpoint_path = "src/checkpoint/model.h5"
vectorizer_checkpoint_path = "src/checkpoint/vectorizer.dump"

dump_file = open(vectorizer_checkpoint_path, 'rb')
(bow_vectorizer, tfreq_vectorizer, tfidf_vectorizer) = pickle.load(dump_file)

model = tf.keras.models.load_model(model_checkpoint_path)


@app.route("/predict", methods=['POST'])
def foo():
    input = json.loads(request.data)
    headlines = [{'BodyID':  0, 'Headlines': input['headline']}]
    bodies = [{'BodyID':  0, 'text': input['body']}]
    raw_data = FNCData(headlines, bodies)

    data = pipeline_test(raw_data,
                         bow_vectorizer,
                         tfreq_vectorizer,
                         tfidf_vectorizer)

    result = model.predict(np.array(data))
    result = np.argmax(result)
    result = {
        0: 'agree',
        1: 'disagree',
        2: 'discuss',
        3: 'unrelated'
    }[result]

    return f'{{"stance":"{result}"}}'


if __name__ == '__main__':
    app.run(port=5002)
