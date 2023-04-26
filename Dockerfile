FROM python:3.9-slim

RUN apt-get update
RUN pip install --upgrade pip

ENV HOME=/opt/usf

RUN mkdir $HOME
WORKDIR $HOME

COPY requirements.txt $HOME
COPY controller $HOME/controller
COPY services $HOME/services
COPY utils $HOME/utils
COPY app.py $HOME

RUN pip install -r requirements.txt

CMD ["gunicorn", "app:app", "-b", "0.0.0.0:8000", "--workers","3"]
