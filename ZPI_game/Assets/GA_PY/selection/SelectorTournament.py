import random
from Circuit import Circuit


class SelectorTournament:

    def __init__(self, size_percentage: float):
        self.size_percentage = size_percentage

    def get(self, generation: list[Circuit]) -> Circuit:  # Circuit
        tournament_size = int(len(generation) * self.size_percentage)
        selected_idxs = random.sample(  # sample
            range(len(generation)), tournament_size  # range
        )
        selected = [generation[i] for i in selected_idxs]  # for in
        return min(selected, key=lambda x: x.score)  # min
