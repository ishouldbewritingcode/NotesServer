# notes server

* This is a simple demo server with authentication only being an email address with no password at this time.
* The notes are associated to the user by email address

---

Tables in the sqllite database are as follows

- user
	- Id [guid]
	- Name [string]
	- Email [string]

- note
	- Id [guid]
	- Title [string]
	- Note [string]
	- UserId [guid]

---

* We're using graphql as API access
* if the user doesn't exist when authenticating, we want to automatically create the user so that they can add notes.
