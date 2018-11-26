# RESTfulOData Service

This service allows the API to filter user requests using OData, and returns a RESTful response. Clone the repository and run the sample project to try it out.

**Sample RESTfulResult Response:**
```
{ 
	"items":[
		{
			"id":"",
			"href":"",
			"created":"",
			"modified":""
		}
	], 
	"length": 1, 
	"href":""
} 
```

## To begin using this service:
- Download the NuGet package
- Add `services.AddRESTfulODataService();` to `Startup.cs`
- Inject  `IODataService`  in Controllers

## How to use this service:
- All resources must implement `IRESTfulItemResult` interface
- Create JsonConverters to remove unwanted properties and to turn relational properties to `href` properties
> Using the example in the sample project, BookJsonConverter converts BookModel 
> From:
> 
```
{ 
	id:  "book1",
	href:  "/books/book1",
	title:  "First Book Title",
	totalPages:  200,
	ratings:  3.5,
	type:  "novel",
	label:  "First Book Label",
	created:  "2018-11-26T04:53:02.0333107Z",
	modified:  "0001-01-01T00:00:00",
	authorId:  "author1"
}
```
> To:
```
{
    id:  "book1",
    href:  "/books/book1",   
    title:  "First Book Title",   
    totalPages:  200,   
    ratings:  3.5,   
    type:  "Novel",   
    label:  "First Book Label",   
    created:  "2018-11-26T04:52:09.6846437Z",   
    modified:  "0001-01-01T00:00:00",   
    readers: {   
        href:  "/books/book1/readers"       
    },
	chapters: {
		href:  "/books/book1/chapters"
    }
}
```
- Add newly created JsonConverters to `Startup.cs`

## Using OData:
Currently supports `$filter` , `$top`, `$skip`, `$orderBy`, and `$expand`

- `$filter`
	- **eq**: Equals
		- `/books?$filter=id eq 'book1'`
		- `/books?$filter=totalPages eq 41`
	-  **ne**: Not equals
		- `/books?$filter=id ne 'book1'`
		- `/books?$filter=totalPages ne 41`
    -  **lt**: Less than
   		- `/books?$filter=ratings lt 2.1` 
		- `/books?$filter=totalPages lt 41`
    - **gt**: Greater than
   		- `/books?$filter=ratings gt 2.1` 
		- `/books?$filter=totalPages gt 41`
    - **ge**: Greater than or Equal to
   		- `/books?$filter=ratings ge 2.1` 
		- `/books?$filter=totalPages ge 41`   
	- **le**: Less than or Equal to 
   		- `/books?$filter=ratings le 2.1` 
		- `/books?$filter=totalPages le 41`     
	- **startswith**
		- `/books?$filter=startswith(id, 'bo')`
		- `/books?$filter=startswith(title, 'F')`
    - **endswith**
		- `/books?$filter=endswith(id, 'k1')`
		- `/books?$filter=endswith(title, 'le')`
    - **contains**
		- `/books?$filter=contains(id, '1')`
		- `/books?$filter=contains(title, 'F')`



You are able to concatenate `$filter` using **and** or **or**.
**Examples**:

`/books?$filter=startswith(title, 'F') and id eq 'book1'`

`/books?$filter=contains(title, 'S') and rating gt 2`

> Use single quotes for string values
---
- `$expand`
Use expand to retrieve nested resources.
	-  `/books?$expand=Chapters` 
	- `/authors?$expand=Books.Chapters`

**Examples:**
```
Request: /authors
Response:
{
	items: [
	{
		id: "author1",
		name: "John Doe",
		href: "/authors/author1",
		created: "0001-01-01T00:00:00",
		modified: "0001-01-01T00:00:00",
		books: {
			href: "/authors/author1/books"
		}
	}],
	length: 1,
	href: "/authors"
}
```

```
Request: /authors?$expand=Books
Response:
{
	items: [
	{
		id: "author1",
		name: "John Doe",
		books: [
			{
				id: "book1",
				title: "First Book Title",
				totalPages: 200,
				ratings: 3.5,
				type: "Novel",
				label: "First Book Label",
				created: "2018-11-26T05:29:45.4838837Z",
				modified: "0001-01-01T00:00:00",
				authorId: "author1"
			}
		]
		href: "/authors/author1",
		created: "0001-01-01T00:00:00",
		modified: "0001-01-01T00:00:00"
	}],
	length: 1,
	href: "/authors?$expand=Books"
}
```

```
Request: /authors?$expand=Books.Chapters
Response:
{
	items: [
	{
		id: "author1",
		name: "John Doe",
		books: [
			{
				id: "book1",
				title: "First Book Title",
				totalPages: 200,
				ratings: 3.5,
				type: "Novel",
				label: "First Book Label",
				created: "2018-11-26T05:29:45.4838837Z",
				modified: "0001-01-01T00:00:00",
				chapters: [
					{
						id: "b1c1",
						name: "First Chapter",
						created: "0001-01-01T00:00:00",
						modified: "0001-01-01T00:00:00",
						bookId: "book1"
					}
				],
				authorId: "author1"
			}
		],
		href: "/authors/author1",
		created: "0001-01-01T00:00:00",
		modified: "0001-01-01T00:00:00"
	}],
	length: 1,
	href: "/authors?$expand=Books.Chapters"
}
```