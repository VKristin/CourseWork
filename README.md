# Обучающая параллелным вычислениям игра, выполненная в рамках курсовой работы

Игра состоит из двух частей:
- Конструктора заданий
- Конструктора алгоритмов

В игре использован псевдопараллелизм, т.е. сама игра не использует параллельные вычисления.

В игре следующий сюжет:
Стройку ведут 3 бригады, которые работают одновременно. Они все могут выполнять одни и те же действия за одно и то же время. Можно считать, что работа ведется тактами. За каждый такт каждая бригада выполняет одно действие. 
Строительство ведется из балок, каждая из которых имеет уникальный номер. 
Каждая балка может быть уложена горизонтально или установлена вертикально. 
Уложить балку можно на землю, на две вертикальные балки, отстоящие друг о друга на длину балки (один край на одну балку, другой – на другую), на одну вертикальную балку (серединой на вершину вертикальной балки).
Установить балку можно на землю, на левый или правый край лежащей балки, на середину лежащей балки.
Точки на земле, между которыми может быть помещена балка, обозначены латинскими буквами. Расстояние между соседними точками равно длине балки.

Задача: 
Разработать наиболее эффективный алгоритм для строительных бригад
