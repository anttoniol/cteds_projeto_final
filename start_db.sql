CREATE TABLE IF NOT EXISTS categories(
	id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	name TEXT NOT NULL,
	color TEXT,
	icon BLOB,
	added_dttm datetime NOT NULL, 
	CONSTRAINT UC_category UNIQUE (name)
);

CREATE TABLE IF NOT EXISTS expenses(
	id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	value DECIMAL NOT NULL,
	desc TEXT,
	category_id INTEGER NOT NULL,
	added_dttm datetime NOT NULL,
	expense_dttm datetime NOT NULL,
	FOREIGN KEY(category_id) REFERENCES categories(id),
	CONSTRAINT UC_expense UNIQUE (desc)

);