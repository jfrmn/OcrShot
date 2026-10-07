# OcrShot - Specification

## Pitch

Sometimes you need to copy the some text on the screen that is not selectable and can't be interacted with at all. That leaves you with typing out the word you want to copy which is not only annoying but also error prone.

This is where OcrShot comes in - a small application that lets the user select a portion of the screen, detects any text within that portion and copies it as unicode string.

## Features

- **Selecting the source area** 
  The user should be able to select a rectangular portion of the screen by clicking + dragging with the mouse. While dragging everything on the screen outside of the source area should be dimmed.
  Within that area the application should look for characters

- **Selecting the output**:
  There are 2 output modi:
   a) Copy to clipboard. The detected text gets copied as plain unicode text
   b) Copy to text box. The Gui of the application should contain a small but multiline text box where that recieves the output gets 

- **User interface**
  Should be minimal. Should contain a button that starts the selection of the source area, and 2 toggle-buttons that determine the output mode. If output mode a) is selected the text box should be hidden.
  Once the source area is selected and the processing start the progress should be indicated by a progress bar (preferred) or a loading spinner.

- **Speed vs Quality** Prefer processing speed over a perfect solution that can handle handwriting and obscured text.

## Technology

- **OS:**  Microsoft Windows, support for other Linux/Mac is not necessary

- Otherwise no rules.
