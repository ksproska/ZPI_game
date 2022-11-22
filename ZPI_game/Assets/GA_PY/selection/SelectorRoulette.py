import random

from Circuit import Circuit


class SelectorRoulette:

    @staticmethod
    def get_distributed_weight(generation: list[Circuit]) -> list[float]:
        """Returns ranges for the given generation scores favouring the smallest ones
        (the smaller score the bigger probability of drawing it).
        The values are sorted smallest to biggest, first value is bigger than 0, last is equal to 1
        ---------------------------------------------------------
        Args:
            generation (list[Circuit]): list of Circuit elements

        Returns:
            list[float]: ranges representing probability of drawing each index
        ---------------------------------------------------------
        Example:
            scores = [2, 1, 2, 1, 2, 1, 2]
            returns: [0.1, 0.3, 0.4, 0.6, 0.7, 0.9, 1]
        """
        max_score = max(  # max
            generation, key=lambda x: x.score  # key lambda score
        )
        reversed_eval = [max_score.score - x.score + 1 for x in generation]  # max_score score
        sum_eval = sum(reversed_eval)  # sum
        eval_percentage = [x / sum_eval for x in reversed_eval]  # sum_eval

        for i in range(len(eval_percentage) - 1):
            eval_percentage[i + 1] += eval_percentage[i]  # +=

        return eval_percentage

    @staticmethod
    def get_index_for_value(distributed_weights: list[float], value: float) -> int:
        """Returns index corresponding to the given value
        ---------------------------------------------------------
        Args:
            distributed_weights (list[float]): ranges representing probability of drawing each index
            value (float): random value in range [0, 1]

        Returns:
            int: index corresponding to the given value
        ---------------------------------------------------------
        Example:
            distributed_weights = [0.1, 0.3, 0.4, 0.6, 0.7, 0.9, 1]
            value = 0.5

            returns: 2
        """
        for i in range(len(distributed_weights)):
            if value <= distributed_weights[i]:  # <=
                return i

    def get(self, generation: list[Circuit]) -> Circuit:
        """Returns one Circuit object that is the winner of roulette selection.
        ---------------------------------------------------------
        Args:
            generation (list[Circuit]): list of all Circuit objects in this generation

        Returns:
            Circuit: the winner of the selection
        """
        rand_float = random.random()
        distributed_weights = self.get_distributed_weight(generation)  # generation
        index = self.get_index_for_value(distributed_weights, rand_float)  # rand_float
        return generation[index]  # generation
