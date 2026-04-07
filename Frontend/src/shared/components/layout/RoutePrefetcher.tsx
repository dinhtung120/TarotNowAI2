"use client";

import { useEffect } from "react";
import { useRouter } from "@/i18n/routing";

const PREFETCH_ROUTES = [
  "/",
  "/reading",
  "/chat",
  "/wallet",
  "/profile",
  "/readers",
  "/collection",
  "/gamification",
  "/subscription",
];

export default function RoutePrefetcher() {
  const router = useRouter();

  useEffect(() => {
    
    const timer = window.setTimeout(() => {
      PREFETCH_ROUTES.forEach((route) => {
        router.prefetch(route);
      });
    }, 2000);

    
    return () => window.clearTimeout(timer);
  }, [router]);

  return null;
}
