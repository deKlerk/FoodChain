# Food Chain plugin for Grasshopper

Food Chain offers a set of components to work with RDF data inside Grasshopper. It uses GhPython Remote plugin to load RDFLib <https://rdflib.readthedocs.io/en/stable/index.html> module in order to load, create, search, edit, etc. data stored in Graphs in RDF format.

It is still in an early stage of development and currently work in progress...

## Using Food Chain

In order to start using Food Chain, you will need to have Python 2.7 installed on your machine and **RDFLib installed on Python 2.7**.
Then, in Grasshopper, place GhPython-Remote component on the canvas, set the location to the folder where python.exe (2.7) is installed and add a panel with the name of the module ("rdflib") to the component's "module" input.

The module will be accessible to all Food Chain components via "scriptcontext.sticky".

![GhPython-Remote with RDFLib](./img/GhPython_Remote.jpg)

## Acknowledgments

**Food Chain** is part of ongoing research by Rui de Klerk, made possible with the financial support from the Portuguese Foundation for Science and Technology (FCT) through the FCT Doctoral Grant SFRH/BD/131386/2017.
