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
];

export default function RoutePrefetcher() {
  const router = useRouter();

  useEffect(() => {
    PREFETCH_ROUTES.forEach((route) => {
      router.prefetch(route);
    });
  }, [router]);

  return null;
}
