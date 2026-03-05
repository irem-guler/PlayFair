# Playfair Cipher (C# WinForms)

This project is a **Windows Forms application** that implements the classical **Playfair Cipher encryption algorithm**.

The program allows users to encrypt and decrypt messages using a keyword-based 5x5 letter matrix.

---

## Features

* Encrypt plaintext using Playfair Cipher
* Decrypt ciphertext using the same key
* Automatic key normalization
* Turkish character normalization (İ → I)
* 5x5 Playfair matrix generation
* Simple graphical interface

---

## Technologies Used

* C#
* .NET Windows Forms
* Visual Studio

---

## How the Playfair Cipher Works

The Playfair cipher is a digraph substitution cipher that uses a **5×5 matrix of letters** generated from a keyword.

Encryption rules:

1. If both letters are in the same row → shift right
2. If both letters are in the same column → shift down
3. Otherwise → form a rectangle and swap columns

The letter **J is replaced with I** to fit the 25-letter matrix.

---

## Running the Project

1. Clone the repository:

```
git clone https://github.com/irem-guler/PlayFair.git
```

2. Open the solution file:

```
PlayFair.sln
```

3. Run the project in **Visual Studio**.

---

## Example

Key:

```
MONARCHY
```

Plaintext:

```
HELLO
```

Ciphertext example:

```
CFSUPM
```

---

## Author

İrem Güler
