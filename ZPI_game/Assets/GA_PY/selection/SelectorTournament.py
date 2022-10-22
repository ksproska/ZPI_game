import random
from Circuit import Circuit


class SelectorTournament:

    def __init__(self, size_percentage):
        self.size_percentage = size_percentage

    def get(self, generation: list) -> Circuit:  # Circuit
        tournament_size = int(len(generation) * self.size_percentage)
        selectedIndexes = random.sample(range(len(generation)), tournament_size)  # sample range
        selected = [generation[i] for i in selectedIndexes]  # for in
        return min(selected, key=lambda x: x.score)  # min
