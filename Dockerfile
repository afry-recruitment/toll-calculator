FROM python:3.9-slim

RUN apt-get update
RUN pip install --upgrade pip

ENV HOME=/opt/usf

RUN mkdir $HOME
WORKDIR $HOME

COPY requirements.txt $HOME
COPY src $HOME/src
COPY app.py $HOME

RUN pip install -r requirements.txt

RUN pip install gunicorn==20.1.0

CMD ["gunicorn", "app:app", "-b", "0.0.0.0:8000", "--workers","3"]
