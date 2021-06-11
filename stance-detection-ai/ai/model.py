import pickle

import numpy as np
from tensorflow.keras.callbacks import ModelCheckpoint
from tensorflow.keras.layers import Dense, Dropout, InputLayer, Softmax
from tensorflow.keras.losses import SparseCategoricalCrossentropy
from tensorflow.keras.models import Sequential
from tensorflow.keras.utils import plot_model

from ai.util import *

def train():
    file_train_instances = "datasets/train_stances.csv"
    file_train_bodies = "datasets/train_bodies.csv"
    file_test_instances = "datasets/test_stances_unlabeled.csv"
    file_test_bodies = "datasets/test_bodies.csv"

    model_checkpoint_path = "checkpoint/model.h5"
    vectorizer_checkpoint_path = "checkpoint/vectorizer.dump"
    img_checkpoint_path = "checkpoint/model.png"

    raw_train = FNCData(file_train_instances, file_train_bodies)
    raw_test = FNCData(file_test_instances, file_test_bodies)

    (train_set,
    train_stances,
    bow_vectorizer,
    tfreq_vectorizer,
    tfidf_vectorizer) = pipeline_train(raw_train, raw_test, lim_unigram=5000)

    dump_file = open(vectorizer_checkpoint_path, 'wb')
    pickle.dump((bow_vectorizer, tfreq_vectorizer, tfidf_vectorizer), dump_file)

    feature_size = len(train_set[0])

    model = Sequential([
        InputLayer(input_shape=[feature_size, ]),
        Dropout(.25),
        Dense(100, activation='relu'),
        Dense(4),
        Softmax()
    ])

    model.compile(optimizer='adam',
                loss=SparseCategoricalCrossentropy(from_logits=True),
                metrics=['accuracy'])

    plot_model(model,
            to_file=img_checkpoint_path,
            show_shapes=True,
            show_layer_names=True)

    cp_callback = ModelCheckpoint(
        filepath=model_checkpoint_path,
        verbose=1)

    model.fit(
        np.array(train_set),
        np.array(train_stances),
        epochs=90,
        verbose=1,
        validation_split=.2,
        callbacks=[cp_callback])
