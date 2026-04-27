import { cn } from "@/lib/utils";
import { resolveParticleClass } from "./config";

interface AstralParticlesProps {
 particleCount: number;
}

export function AstralParticles({ particleCount }: AstralParticlesProps) {
 return (
  <div className={cn("absolute inset-0 motion-reduce:hidden astral-heavy")}>
   {Array.from({ length: particleCount }).map((_, index) => (
    <div
     key={`astral-particle-${index}`}
     className={cn("absolute tn-size-2px tn-bg-holo-silver rounded-full animate-float tn-opacity-40 tn-shadow-particle", resolveParticleClass(index))}
    />
   ))}
  </div>
 );
}
