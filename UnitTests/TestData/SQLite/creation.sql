CREATE TABLE Categories(id int, description text);

CREATE TABLE Genders(id int, description text);

CREATE TABLE Places(id int, description text, idcountry int, country text, idregion int, region text);

CREATE TABLE Sales(category int, gender int, place int, date text, expenses real, items int);