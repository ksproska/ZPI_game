from flask import Flask
from db_models import get_db
from routes import get_routes
from data_model_schemas import get_marshmallow

app = Flask(__name__)

app.config["SQLALCHEMY_DATABASE_URI"] = "sqlite:///project.db"
db = get_db()
db.init_app(app)

routes = get_routes()
app.register_blueprint(routes)

fl_marshmallow = get_marshmallow()
fl_marshmallow.init_app(app)

if __name__ == '__main__':
    with app.app_context():
        db.create_all()
    app.run(host='0.0.0.0', port=5000)

