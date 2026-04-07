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
   <div className={cn("absolute inset-0 opacity-100 bg-[radial-gradient(120%_90%_at_10%_0%,var(--c-215-189-226-50)_0%,transparent_54%),radial-gradient(90%_75%_at_85%_8%,var(--c-178-232-214-42)_0%,transparent_56%),radial-gradient(100%_80%_at_50%_102%,var(--c-255-250-205-36)_0%,transparent_60%)]")} />
   <AstralNebula variant={variant} tone={tone} />
   {variant === "intense" ? <div className={cn("absolute inset-0 opacity-[0.08] bg-[linear-gradient(to_right,var(--c-hex-a89cff28)_1px,transparent_1px),linear-gradient(to_bottom,var(--c-hex-b2e8d628)_1px,transparent_1px)] bg-[size:64px_64px]")} /> : null}
   <AstralParticles particleCount={resolvedParticleCount} />
  </div>
 );
}
