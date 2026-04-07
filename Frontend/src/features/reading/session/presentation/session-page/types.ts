export interface FlyingCard {
  key: string;
  startX: number;
  startY: number;
  deltaX: number;
  deltaY: number;
  rotate: number;
  stackIndex: number;
}

export interface ShufflePath {
  tx: string;
  ty: string;
  r: string;
  tx2: string;
  ty2: string;
  r2: string;
  delay: string;
  duration: string;
  z: number;
}
