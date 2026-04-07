import { cn } from "@/lib/utils";
import { getParticleStyle } from "./config";

interface AstralParticlesProps {
 particleCount: number;
}

export function AstralParticles({ particleCount }: AstralParticlesProps) {
 return (
  <div className={cn("absolute inset-0 motion-reduce:hidden astral-heavy")}>
   {Array.from({ length: particleCount }).map((_, index) => (
    <div
     key={`astral-particle-${index}`}
     className={cn("absolute w-[2px] h-[2px] bg-[var(--holo-silver)] rounded-full animate-float opacity-[0.4] shadow-[0_0_10px_var(--c-168-156-255-55)]")}
     style={getParticleStyle(index)}
    />
   ))}
  </div>
 );
}
