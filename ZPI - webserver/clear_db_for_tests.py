import sqlite3

con = sqlite3.connect(r".\flask-app\instance\project.db")

cur = con.cursor()

cur.execute("DELETE FROM USERS")

cur.execute("DELETE FROM MAPS")

cur.execute("DELETE FROM POINTS")

cur.execute("DELETE FROM SCORES")

con.commit()
con.close()
