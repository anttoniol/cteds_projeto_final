CREATE TABLE IF NOT EXISTS categories(
	id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	name TEXT NOT NULL,
	deleted_dttm datetime DEFAULT (DATETIME('0')),
	CONSTRAINT UC_category UNIQUE (name, deleted_dttm)
);

CREATE TABLE IF NOT EXISTS expenses(
	id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	value NUMERIC NOT NULL default 0,
	desc TEXT,
	category_id INTEGER NOT NULL,
	added_dttm datetime NOT NULL,
	FOREIGN KEY(category_id) REFERENCES categories(id)
);