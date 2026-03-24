"use client";

import { useEffect } from "react";
import { getWalletBalance } from "@/features/wallet/application/actions";
import { setWalletBalanceFetcher } from "@/store/walletStore";

export default function WalletStoreBridge() {
 useEffect(() => {
  setWalletBalanceFetcher(getWalletBalance);
  return () => {
   setWalletBalanceFetcher();
  };
 }, []);

 return null;
}
