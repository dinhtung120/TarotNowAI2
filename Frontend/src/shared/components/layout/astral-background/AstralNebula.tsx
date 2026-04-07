import { cn } from "@/lib/utils";
import type { AstralVariant } from "./config";

interface AstralNebulaProps {
 variant: AstralVariant;
 tone: {
  purple: string;
  mint: string;
  moon: string;
 };
}

export function AstralNebula({ variant, tone }: AstralNebulaProps) {
 const purpleBlobClass = variant === "subtle" ? "absolute top-[-14%] -left-[18%] w-[60vw] h-[60vw] blur-[90px] rounded-full motion-reduce:animate-none" : "absolute top-[-20%] -left-1/4 w-[80vw] h-[80vw] blur-[150px] rounded-full motion-reduce:animate-none";
 const mintBlobClass = variant === "subtle" ? "absolute top-[16%] -right-[18%] w-[52vw] h-[52vw] blur-[80px] rounded-full motion-reduce:animate-none" : "absolute top-1/4 -right-1/4 w-[70vw] h-[70vw] blur-[140px] rounded-full motion-reduce:animate-none";

 return (
  <>
   <div className={cn(purpleBlobClass, "animate-drift astral-heavy")} style={{ backgroundColor: tone.purple }} />
   <div className={cn(mintBlobClass, "animate-drift-reverse astral-heavy")} style={{ backgroundColor: tone.mint }} />
   {variant !== "subtle" ? <div className={cn("absolute -bottom-1/4 left-1/3 tn-size-60vw tn-blur-130 rounded-full animate-slow-pulse motion-reduce:animate-none astral-heavy")} style={{ backgroundColor: tone.moon }} /> : null}
  </>
 );
}
