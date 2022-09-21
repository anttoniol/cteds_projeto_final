CREATE TABLE categories(
	id INTEGER NOT NULL,
	name TEXT NOT NULL,
	deleted_dttm datetime NOT NULL default (DATETIME('0')),
	CONSTRAINT PK_category PRIMARY KEY (id),
	CONSTRAINT UC_category UNIQUE (name, deleted_dttm)
);

CREATE TABLE expenses(
	id INTEGER NOT NULL,
	value NUMERIC NOT NULL default 0,
	desc TEXT,
	category_id INTEGER NOT NULL,
	added_dttm datetime not null default (datetime('0')),
	CONSTRAINT PK_expenses PRIMARY KEY (id),
	FOREIGN KEY(category_id) REFERENCES categories(id)
);

