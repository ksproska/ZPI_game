from flask_sqlalchemy import SQLAlchemy
from hashlib import sha256

db = SQLAlchemy()

class Users(db.Model):
    user_id = db.Column(db.Integer, primary_key=True)
    email = db.Column(db.String(200), nullable=False)
    nickname = db.Column(db.String(30), nullable=False)
    password = db.Column(db.String(64), nullable=False)

    def __init__(self, email, nickname, password) -> None:
        sha_engine = sha256()
        sha_engine.update(password.encode(encoding='utf-8'))

        self.email = email
        self.nickname = nickname
        self.password = sha_engine.hexdigest()

class Maps(db.Model):
    map_id = db.Column(db.Integer, primary_key=True)
    user_id = db.Column(db.Integer, db.ForeignKey('users.user_id'), nullable=True)
    creation_date = db.Column(db.Date, nullable=True)
    user = db.relationship('Users', backref=db.backref('maps',lazy=True), uselist=False)

    def __init__(self, user_id, creation_date) -> None:
        self.user_id = user_id
        self.creation_date = creation_date

class Points(db.Model):
    map_id = db.Column(db.Integer, db.ForeignKey('maps.map_id'), primary_key=True, autoincrement=False)
    X = db.Column(db.Float, primary_key=True)
    Y = db.Column(db.Float, primary_key=True)
    map = db.relationship('Maps', backref=db.backref('points',lazy=True), uselist=False)

    def __init__(self, map_id, X, Y) -> None:
        self.map_id = map_id
        self.X = X
        self.Y = Y

class Scores(db.Model):
    map_id = db.Column(db.Integer, db.ForeignKey('maps.map_id'), primary_key=True, autoincrement=False)
    user_id = db.Column(db.Integer, db.ForeignKey('users.user_id'), primary_key=True, autoincrement=False)
    score = db.Column(db.Float, nullable=False)
    map = db.relationship('Maps', backref=db.backref('scores',lazy=True), uselist=False)
    user = db.relationship('Users', backref=db.backref('scores',lazy=True), uselist=False)

    def __init__(self, map_id, user_id, score) -> None:
        self.map_id = map_id
        self.user_id = user_id
        self.score = score

def get_db():
    return db