<div align="center">
    <h3>XML Validator Library</h3>
    <p>Stop wasting time Googling "XML validator" and start using your console. It's as easy as ctrl + c and ctrl + v!</p>
</div>

## About the Project

XML Validator Library is a lightweight CLI that can quickly validate XML.

## Examples

Using the checker.exe, an XML string can be validated like so:

```sh
> checker.exe "<xml></xml>"
Valid
```

```sh
> checker.exe "<Design><Code>hello world</Code></Design>"
Valid
```

```sh
> checker.exe "<Design><Code>hello world</Code></Design><People>"
Invalid
```

```sh
> checker.exe "<People><Design><Code>hello world</People><Code></Design>"
Invalid
```

Tags with attributes will need to have the same attributes on their opening and closing tags:

```sh
> checker.exe "<People age="1">hello world</People>"
Invalid
```

```sh
> checker.exe "<People age="1">hello world</People age="1">"
Valid
```
