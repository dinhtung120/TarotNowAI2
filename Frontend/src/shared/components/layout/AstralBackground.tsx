"use client";

import { cn } from "@/lib/utils";
import { AstralNebula } from "./astral-background/AstralNebula";
import { AstralParticles } from "./astral-background/AstralParticles";
import { getNebulaTone, getResolvedParticleCount, type AstralVariant } from "./astral-background/config";

interface AstralBackgroundProps {
 variant?: AstralVariant;
 particleCount?: number;
}

export default function AstralBackground({ variant = "default", particleCount }: AstralBackgroundProps) {
 const tone = getNebulaTone(variant);
 const resolvedParticleCount = getResolvedParticleCount(variant, particleCount);

 return (
  <div className={cn("fixed inset-0 z-0 pointer-events-none astral-background")} aria-hidden="true">
   <div className={cn("absolute inset-0 opacity-100 tn-astral-glow-layer")} />
   <AstralNebula variant={variant} tone={tone} />
   {variant === "intense" ? <div className={cn("absolute inset-0 tn-astral-grid-overlay")} /> : null}
   <AstralParticles particleCount={resolvedParticleCount} />
  </div>
 );
}
