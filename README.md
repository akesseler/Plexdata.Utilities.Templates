<p align="center">
  <a href="https://github.com/akesseler/Plexdata.Utilities.Templates/blob/main/LICENSE.md" alt="license">
    <img src="https://img.shields.io/github/license/akesseler/Plexdata.Utilities.Templates.svg" />
  </a>
  <a href="https://github.com/akesseler/Plexdata.Utilities.Templates/releases/latest" alt="latest">
    <img src="https://img.shields.io/github/release/akesseler/Plexdata.Utilities.Templates.svg" />
  </a>
  <a href="https://github.com/akesseler/Plexdata.Utilities.Templates/archive/main.zip" alt="main">
    <img src="https://img.shields.io/github/languages/code-size/akesseler/Plexdata.Utilities.Templates.svg" />
  </a>
  <a href="https://akesseler.github.io/Plexdata.Utilities.Templates" alt="docs">
    <img src="https://img.shields.io/badge/docs-guide-orange.svg" />
  </a>
  <a href="https://github.com/akesseler/Plexdata.Utilities.Templates/wiki" alt="wiki">
    <img src="https://img.shields.io/badge/wiki-API-orange.svg" />
  </a>
</p>

## Plexdata Template Formatter

The _Plexdata Template Formatter_ represents an alternative to `String.Format()`, but with template support.

Main feature of this library is that template formatting is possible beside standard formatting, as it is done by `String.Format()`.

As extension to `String.Format()`, _stringification_ as well as _serialization_ operations of index–based format arguments are supported as well.

More information about template formatting can be found on the Internet under [https://messagetemplates.org](https://messagetemplates.org).

### Examples

Here are some examples of how to use this template formatter.

Below find a typical string format using the standard .NET Framework function `String.Format()`.

```
String.Format("Invoice for customer {0} for the sum of {1:C2}.", "John Doe", 123.45)
```

Now, the same as above but with this template formatter using standard format instructions as well as template format instructions.

```
Template.Format("Invoice for customer {0} for the sum of {1:C2}.", "John Doe", 123.45)
Template.Format("Invoice for customer {customer} for the sum of {totalsum:C2}.", "John Doe", 123.45)
```

But there are many more features available. Therefore, please read the documentation, that can be found [here](https://akesseler.github.io/Plexdata.Utilities.Templates/).

### Licensing

The software has been published under the terms of _MIT License_.

### Downloads

The latest release can be obtained from [https://github.com/akesseler/plexdata.utilities.templates/releases/latest](https://github.com/akesseler/Plexdata.Utilities.Templates/releases/latest).

The main branch can be downloaded as ZIP from [https://github.com/akesseler/plexdata.utilities.templates/archive/main.zip](https://github.com/akesseler/Plexdata.Utilities.Templates/archive/main.zip).


### Documentation

The documentation with an overview, an introduction as well as examples is available under [https://akesseler.github.io/plexdata.utilities.templates](https://akesseler.github.io/Plexdata.Utilities.Templates/).

The full API documentation is available as Wiki and can be read under [https://github.com/akesseler/plexdata.utilities.templates/wiki](https://github.com/akesseler/Plexdata.Utilities.Templates/wiki).

