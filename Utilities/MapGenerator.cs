using System;
using System.Collections.Generic;

public class MapGenerator {
    private class Node {
        public int x, y, region;
        public int dist;
    }

    private int width, height;
    private Random rng;

    public MapGenerator(int width, int height) {
        this.width = width;
        this.height = height;
        this.rng = new Random();
    }

    public Tile[,] Generate() {
        // Derive buffer radius and seed spacing from map size
        int minSide = Math.Min(width, height);
        float areaPerRegion = (width * height) / 4f;

        float regionRadius = MathF.Sqrt(areaPerRegion / MathF.PI);

        // seed spacing (unchanged idea)
        float minDistance = regionRadius * 1.0f;
        int minDistSq = (int)(minDistance * minDistance);

        // buffer derived ONLY from map scale
        int buffer = Math.Max(1, (int)(minSide * 0.1f));

        if (minSide < 20) {
            buffer = 1;
        }

        // Step 1: Pick seeds with minimum distance constraint
        var seeds = PickSeeds(regionRadius);

        // Step 2: Expand regions with water buffer
        var owner = ExpandRegions(seeds, buffer);

        // Step 3: Convert to tile map
        var map = new Tile[width, height];
        Tile[] regionTiles = { Tile.Region0, Tile.Region1, Tile.Region2, Tile.Region3 };
        
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (owner[x, y] == -1) {
                    map[x, y] = Tile.Water;
                } else {
                    map[x, y] = regionTiles[owner[x, y]];
                }
            }
        }

        return map;
    }

    private List<(int x, int y)> PickSeeds(float regionRadius) {
        float minDistance = regionRadius * 2f * 0.5f;
        int minDistSq = (int)(minDistance * minDistance);

        var seeds = new List<(int x, int y)>();

        while (seeds.Count < 4) {
            int x = rng.Next(width);
            int y = rng.Next(height);

            bool valid = true;

            foreach (var s in seeds) {
                int dx = s.x - x;
                int dy = s.y - y;
                if (dx * dx + dy * dy < minDistSq) {
                    valid = false;
                    break;
                }
            }

            if (valid) {
                seeds.Add((x, y));
            }
        }

        return seeds;
    }

    private int[,] ExpandRegions(List<(int x, int y)> seeds, int buffer) {
        var owner = new int[width, height];
        var dist = new int[width, height];

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                owner[x, y] = -1;
                dist[x, y] = int.MaxValue;
            }
        }

        var pq = new PriorityQueue<Node, int>();

        // Initialize seeds
        for (int i = 0; i < seeds.Count; i++) {
            var s = seeds[i];
            var node = new Node { x = s.x, y = s.y, region = i, dist = 0 };
            pq.Enqueue(node, 0);
            dist[s.x, s.y] = 0;
            owner[s.x, s.y] = i;
        }

        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };

        while (pq.Count > 0) {
            var node = pq.Dequeue();

            if (node.dist > dist[node.x, node.y]) {
                continue;
            }

            for (int k = 0; k < 4; k++) {
                int nx = node.x + dx[k];
                int ny = node.y + dy[k];

                if (nx < 0 || ny < 0 || nx >= width || ny >= height) {
                    continue;
                }

                if (owner[nx, ny] != -1) {
                    continue;
                }

                if (!IsSafe(nx, ny, owner, node.region, buffer)) {
                    continue;
                }

                owner[nx, ny] = node.region;
                dist[nx, ny] = node.dist + 1;

                pq.Enqueue(new Node {
                    x = nx,
                    y = ny,
                    region = node.region,
                    dist = node.dist + 1
                }, node.dist + 1);
            }
        }

        return owner;
    }

    private bool IsSafe(int x, int y, int[,] owner, int region, int buffer) {
        for (int dx = -buffer; dx <= buffer; dx++) {
            for (int dy = -buffer; dy <= buffer; dy++) {
                int nx = x + dx;
                int ny = y + dy;

                if (nx < 0 || ny < 0 || nx >= width || ny >= height) {
                    continue;
                }

                int r = owner[nx, ny];

                if (r != -1 && r != region) {
                    return false;
                }
            }
        }

        return true;
    }

    private float Clamp(float value, float min, float max) {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }
}
