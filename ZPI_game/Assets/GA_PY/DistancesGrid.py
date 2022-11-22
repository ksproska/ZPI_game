class DistancesGrid:
    def __init__(self, distances_grid: list[list]):   # __init__
        """Object stores distances between cities as a grid each to each.
        ---------------------------------------------------------
        Args:
            distances_grid (list[list]): list of lists of distances between cities.
        ---------------------------------------------------------
        Example:
            distances_grid = [
                [0, 1, 4, 2],
                [1, 0, 3, 6],
                [4, 3, 0, 8],
                [2, 6, 8, 0]
            ]
            number_of_cities = 4
        """
        self.distances_grid: list[list] = distances_grid   # list
        self.number_of_cities: int = len(distances_grid)    # len

    def get_distance(self, index1: int, index2: int) -> float:   # float
        """Method returns distance between two cites, where cities are represented by index <0, x>.
        ---------------------------------------------------------
        Args:
            index1 (int): index in grid of the first city
            index2 (int): index in grid of second city

        Returns:
            float: distance between two cities for given indexes
        ---------------------------------------------------------
        Example:
            distances_grid = [
                [0, 1, 4, 2],
                [1, 0, 3, 6],
                [4, 3, 0, 8],
                [2, 6, 8, 0]
            ]
            get_weight(0, 1) = 1
            get_weight(1, 0) = 1
            get_weight(2, 2) = 0
        """
        return self.distances_grid[index1][index2]  # distances_grid
