import pyodbc

def querydb(querystring):
    dbname = 'CommerceBankProject'
    conn = pyodbc.connect(driver='{SQL Server Native Client 11.0}', server='(localdb)\MSSqlLocalDb', database=dbname, trusted_connection='yes')
    cursor = conn.cursor()
    cursor.execute(querystring)
    cursor.commit()
    conn.close()
    

if __name__ == '__main__':
    query = "SET IDENTITY_INSERT [Transaction] ON;\n"
    fh = open("ProjectData.csv", "r")
    data = fh.readlines()
    fh.close()
    customers = []
    emails = []
    count = 100000000
    for line in data:
        count += 1
        query += "INSERT INTO [Transaction](ID, customerID, actID, actType, onDate, balance, transType, amount, description, userEntered) VALUES ("
        fields = line.split(",")
        cust = fields[7]
        if cust not in customers:
            customers.append(cust)
            emails.append(fields[8][:-1])
        query += str(count) + ", " + cust + ", " + fields[1] + ", '" + fields[0] + "', "
        date = fields[2].split("/")
        query += "'%s-%s-%s', " % (date[2], date[0], date[1])
        query += fields[3] + ", '" + fields[4] + "', " + fields[5][1:-1] + ", "
        query += "'" + fields[6].replace("'", "''") + "', 0);\n"
    query += "SET IDENTITY_INSERT [Transaction] OFF;"
    #querydb(query)
    query = "Drop table [Date]; Drop table [Account];"
    #querydb(query)
    query = ""
    for i in range(len(customers)):
        query += "INSERT INTO [Customer](customerID, email) VALUES ('"
        query += customers[i] + "', '" + emails[i] + "');\n"
    querydb(query)