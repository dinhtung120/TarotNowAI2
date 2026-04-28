export interface CardOptionStats {
 level: number;
 currentExp: number;
 expToNextLevel: number;
 totalAtk: number;
 totalDef: number;
}

export interface CardOption {
 id: number;
 name: string;
 imageUrl?: string;
 stats?: CardOptionStats;
}
