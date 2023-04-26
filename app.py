import os
import logging
from flask import Flask, Response
from flask_restx import Api
from logging.config import dictConfig

from controller.echo import echo_ns

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

api = Api(title='Toll Fee API', description='Toll fee calculator 1.0',)
api.init_app(app)
api.add_namespace(echo_ns, path='/api')

if __name__ == '__main__':
    logging.info('Toll Fee service starting ...')
    app.run(host=os.getenv('TOLL_FEE_IP_ADDRESS', '0.0.0.0'), port=3000, debug=False)
