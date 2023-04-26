import logging
import os
from logging.config import dictConfig

from flask import Flask
from flask_mongoengine import MongoEngine
from flask_restx import Api

from controller.echo import echo_ns
from controller.toll_fee import tollfee_ns

db = MongoEngine()

dictConfig({
    'version': 1,
    'formatters': {'default': {
        'format': '[%(asctime)s] %(levelname)s in %(module)s: %(message)s',
    }},
    'handlers': {'wsgi': {
        'class': 'logging.StreamHandler',
        'stream': 'ext://flask.logging.wsgi_errors_stream',
        'formatter': 'default'
    }},
    'root': {
        'level': os.environ.get('TOLL_FEE_LOG_LEVEL', 'INFO'),
        'handlers': ['wsgi']
    }
})

app = Flask(__name__)

api = Api(title='Toll Fee API', description='Toll fee calculator 1.0', )
api.init_app(app)
api.add_namespace(echo_ns, path='/api')
api.add_namespace(tollfee_ns, path='/api/v1')


def create_app(app):
    app.config['MONGODB_SETTINGS'] = {
        'db': os.getenv('MONGO_DATABASE'),
        'host': os.getenv('MONGO_HOST'),
        'port': int(os.getenv('MONGO_PORT')),
        'username': os.getenv('MONGO_USERNAME'),
        'password': os.getenv('MONGO_PASSWORD')
    }
    db.init_app(app)
    return app


app = create_app(app)

if __name__ == '__main__':
    logging.info('Toll Fee service starting ...')
    app.run(host=os.getenv('TOLL_FEE_IP_ADDRESS', '0.0.0.0'), port=3000, debug=False)
