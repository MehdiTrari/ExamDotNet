# Labyrinthe (Exercice 2)

Resolution d'un labyrinthe en TDD.

## Contenu
- `MazeSolver` : bibliotheque avec l'algorithme (parsing, voisins, fill, distance, chemin).
- `MazeRunner` : application console pour tester un labyrinthe en entree.
- `Labyrinthe.Tests` : tests unitaires.

## Format d'entree
- `D` : depart
- `S` : sortie
- `#` : mur
- `.` : case libre

Exemple :
```
D..#.
##...
.#.#.
...#.
####S
```

## Lancer le runner
```bash
dotnet run --project /home/mehdi/ExamDotNet/Labyrinthe/src/MazeRunner
```

Exemples integres :
```bash
dotnet run --project /home/mehdi/ExamDotNet/Labyrinthe/src/MazeRunner -- base

dotnet run --project /home/mehdi/ExamDotNet/Labyrinthe/src/MazeRunner -- large
```

Avec un fichier :
```bash
dotnet run --project /home/mehdi/ExamDotNet/Labyrinthe/src/MazeRunner /chemin/vers/maze.txt
```

Exemple complet (creer un fichier puis lancer) :
```bash
cat <<'EOF' > /tmp/maze.txt
D..#.
##...
.#.#.
...#.
####S
EOF

dotnet run --project /home/mehdi/ExamDotNet/Labyrinthe/src/MazeRunner /tmp/maze.txt
```

## Tests
```bash
dotnet test /home/mehdi/ExamDotNet/Labyrinthe/Labyrinthe.sln
```
