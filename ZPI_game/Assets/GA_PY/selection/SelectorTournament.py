import random
from Circuit import Circuit


class SelectorTournament:

    def __init__(self, size_percentage: float):
        """Init for Tournament Selector
        ---------------------------------------------------------
        Args:
            size_percentage (float): the percentage of generation that will participate in tournament
        ---------------------------------------------------------
        Example:
            generation_size = 50
            and
            size_percentage = 0.44 (44%)
            then:
            numb_participating = 22
        """
        self.size_percentage = size_percentage

    def get(self, generation: list[Circuit]) -> Circuit:  # Circuit
        """Returns one Circuit object that is the winner of tournament selection.
        It randomly chooses the participants (of size = len(generation) * self.size_percentage)
        and returns the best one.
        ---------------------------------------------------------
        Args:
            generation (list[Circuit]): list of all Circuit objects in this generation

        Returns:
            Circuit: the winner of the selection
        """
        tournament_size = int(len(generation) * self.size_percentage)
        selected_idxs = random.sample(  # sample
            range(len(generation)), tournament_size  # range
        )
        selected = [generation[i] for i in selected_idxs]  # for in
        return min(selected, key=lambda x: x.score)  # min
