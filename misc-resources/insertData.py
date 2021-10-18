import pyodbc
from random import randint
import datetime

def querydb(querystring):
    #conn = pyodbc.connect('Driver={ODBC Driver 17 for SQL Server};Server=tcp:corner-club-sql.database.windows.net,1433;Database=corner-club-db;Uid=cornerclub;Pwd={90Degrees};Encrypt=yes;TrustServerCertificate=no;Connection Timeout=30;')
    dbname = 'CommerceBankProject'
    conn = pyodbc.connect(driver='{SQL Server Native Client 11.0}', server='(localdb)\MSSqlLocalDb', database=dbname, trusted_connection='yes')
    cursor = conn.cursor()
    cursor.execute(querystring)
    cursor.commit()
    conn.close()
    

if __name__ == '__main__':
    fh = open("ProjectData.csv", "r")
    data = fh.readlines()
    fh.close()
    query = ""
    create = True
    while create:
        try:
            customers = []
            emails = []
            previous = False
            hour = 0
            minute = 0
            second = 0
            for line in data:
                query += "INSERT INTO [Transaction](customerID, actID, actType, onDate, balance, transType, amount, description, userEntered, category) VALUES ("
                fields = line.split(",")
                cust = fields[7]
                if cust not in customers:
                    customers.append(cust)
                    emails.append(fields[8])
                query += "'" + cust + "', '" + fields[1] + "', '" + fields[0] + "', "
                date = fields[2].split("/")
                current = datetime.date(int(date[2]), int(date[0]), int(date[1]))
                if previous:
                    if previous < current:
                        hour = 0
                        minute = 0
                        second = 0
                    else:
                        if hour < 23:
                            hour += 1
                            minute = 0
                            second = 0
                        elif minute < 59:
                            minute += 1
                            second = 0
                        elif second < 59:
                            second += 1
                        else:
                            raise Exception("Error with random time generation")
                hour = randint(hour,23)
                minute = randint(minute,59)
                second = randint(second,59)
                query += "'%s-%s-%s %d:%d:%d', " % (date[2], date[0], date[1], hour, minute, second)
                query += fields[3] + ", '" + fields[4] + "', " + fields[5][1:-1] + ", "
                query += "'" + fields[6].replace("'", "''") + "', 0,"
                query += "'" + fields[9][:-1] + "'" + ");\n" 
                previous = current
            create = False
        except Exception as e:
            print(e)
    query += query[:-1]
    querydb(query)
    query = "Drop table [Date]; Drop table [Account];"
    querydb(query)
    query = ""
    for i in range(len(customers)):
        query += "INSERT INTO [Customer](customerID, email, claimed) VALUES ('"
        query += customers[i] + "', '" + emails[i] + "', 0);\n"
    querydb(query)