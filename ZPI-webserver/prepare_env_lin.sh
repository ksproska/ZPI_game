#!/bin/bash

mkdir flask-venv
cd flask-venv
python3 -m venv .
./Scripts/activate
pip3 install -r ../requirements.txt

