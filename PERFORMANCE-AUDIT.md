# PERFORMANCE AUDIT - TarotNow

## Executive Summary

- Generated at (UTC): 2026-05-09T00:26:11.900Z
- Benchmark generated at (UTC): 2026-05-09T00:26:06.346Z
- Benchmark input: Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark.json
- Base origin: https://www.tarotnow.xyz
- Locale prefix: /vi
- Benchmark mode: full-matrix
- Total scenarios: 6
- Total pages measured: 172
- Critical pages: 2
- High pages: 144
- Medium pages: 26
- Slow requests >800ms: 58
- Slow requests 400-800ms: 345
- Request thresholds: >25 suspicious, >35 severe
- Slow request thresholds: >400ms optimize, >800ms serious

## Scenario Coverage

| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Failed | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 24.9 | 2886 | 0 | 0 | yes |
| logged-in-admin | desktop | 44 | 28.9 | 2953 | 0 | 0 | yes |
| logged-in-reader | desktop | 34 | 28.5 | 2926 | 0 | 0 | yes |
| logged-out | mobile | 9 | 25.1 | 2857 | 0 | 0 | yes |
| logged-in-admin | mobile | 43 | 29.7 | 3058 | 0 | 0 | yes |
| logged-in-reader | mobile | 33 | 29.1 | 2960 | 0 | 0 | yes |

## Route Family Coverage

| Scenario | Viewport | Family | Pages | Avg requests/page | Avg nav (ms) | Pending | Failed |
| --- | --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.6 | 2927 | 0 | 0 |
| logged-in-admin | desktop | auth-public | 3 | 25.0 | 2655 | 0 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | 3 | 32.0 | 3498 | 0 | 0 |
| logged-in-admin | desktop | home | 1 | 26.0 | 2695 | 0 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | 4 | 30.5 | 3732 | 0 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | 8 | 28.6 | 2828 | 0 | 0 |
| logged-in-admin | desktop | reader-chat | 9 | 28.2 | 2816 | 0 | 0 |
| logged-in-admin | desktop | reading | 6 | 29.2 | 2772 | 0 | 0 |
| logged-in-admin | mobile | admin | 10 | 29.4 | 2848 | 0 | 0 |
| logged-in-admin | mobile | auth-public | 3 | 25.0 | 2762 | 0 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | 3 | 32.7 | 3552 | 0 | 0 |
| logged-in-admin | mobile | home | 1 | 34.0 | 2941 | 0 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | 4 | 29.8 | 3630 | 0 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | 8 | 32.3 | 3199 | 0 | 0 |
| logged-in-admin | mobile | reader-chat | 9 | 28.2 | 2984 | 0 | 0 |
| logged-in-admin | mobile | reading | 5 | 29.2 | 2836 | 0 | 0 |
| logged-in-reader | desktop | auth-public | 3 | 25.0 | 2711 | 0 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | 3 | 31.3 | 3090 | 0 | 0 |
| logged-in-reader | desktop | home | 1 | 26.0 | 2765 | 0 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | 4 | 30.0 | 3700 | 0 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | 8 | 28.5 | 2836 | 0 | 0 |
| logged-in-reader | desktop | reader-chat | 9 | 28.0 | 2781 | 0 | 0 |
| logged-in-reader | desktop | reading | 6 | 29.2 | 2801 | 0 | 0 |
| logged-in-reader | mobile | auth-public | 3 | 25.0 | 2735 | 0 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | 3 | 31.7 | 3129 | 0 | 0 |
| logged-in-reader | mobile | home | 1 | 26.0 | 2878 | 0 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | 4 | 31.8 | 3577 | 0 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | 8 | 28.9 | 2885 | 0 | 0 |
| logged-in-reader | mobile | reader-chat | 9 | 28.3 | 2827 | 0 | 0 |
| logged-in-reader | mobile | reading | 5 | 30.4 | 2875 | 0 | 0 |
| logged-out | desktop | auth-public | 8 | 24.4 | 2738 | 0 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 4065 | 0 | 0 |
| logged-out | mobile | auth-public | 8 | 24.5 | 2773 | 0 | 0 |
| logged-out | mobile | home | 1 | 30.0 | 3529 | 0 | 0 |

## Detailed Metrics Table

| Severity | Feature | Scenario | Viewport | Route | Requests | API | Static | Third-party | Nav (ms) | FCP (ms) | LCP (ms) | TBT (ms) | CLS | Pending | Failed | Transfer bytes |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 59 | 2 | 53 | 0 | 4718 | 648 | 1388 | 0.0 | 0.0071 | 0 | 0 | 1254564 |
| Critical | community-leaderboard-quest | logged-in-admin | mobile | /vi/community | 36 | 2 | 32 | 0 | 3935 | 704 | 2156 | 0.0 | 0.0051 | 0 | 0 | 777655 |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/community | 35 | 1 | 32 | 0 | 3999 | 512 | 2112 | 0.0 | 0.0041 | 0 | 0 | 777103 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/community | 35 | 1 | 32 | 0 | 3620 | 480 | 1656 | 0.0 | 0.0039 | 0 | 0 | 776934 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha | 35 | 0 | 33 | 0 | 2954 | 444 | 852 | 0.0 | 0.0069 | 0 | 0 | 796615 |
| High | community-leaderboard-quest | logged-in-reader | mobile | /vi/community | 35 | 1 | 32 | 0 | 3657 | 484 | 1608 | 0.0 | 0.0051 | 0 | 0 | 776966 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/dc12ad57-48d6-4e3f-bfa2-8d84f255a2d2 | 34 | 2 | 30 | 0 | 2919 | 476 | 868 | 0.0 | 0.0044 | 0 | 0 | 724578 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/a9542453-e3a0-4868-96e9-50386f83dd83 | 34 | 2 | 30 | 0 | 2921 | 500 | 868 | 0.0 | 0.0039 | 0 | 0 | 725015 |
| High | auth-public | logged-in-admin | mobile | /vi | 34 | 4 | 27 | 0 | 2941 | 516 | 872 | 0.0 | 0.0032 | 0 | 0 | 609946 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/8d742e33-9bf0-462c-b6c3-4402e7681c13 | 33 | 2 | 29 | 0 | 2960 | 484 | 824 | 0.0 | 0.0000 | 0 | 0 | 692616 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/09f5d83e-ad26-4930-8cbb-13ce88c66000 | 33 | 2 | 29 | 0 | 2946 | 476 | 824 | 0.0 | 0.0000 | 0 | 0 | 693217 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/70126a07-9e85-4898-8b64-061f6d14e550 | 33 | 2 | 29 | 0 | 2982 | 508 | 880 | 0.0 | 0.0000 | 0 | 0 | 692715 |
| High | admin | logged-in-admin | desktop | /vi/admin/gamification | 32 | 0 | 30 | 0 | 3261 | 548 | 840 | 0.0 | 0.0022 | 0 | 0 | 697782 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 32 | 2 | 27 | 0 | 3474 | 748 | 1264 | 0.0 | 0.0071 | 0 | 0 | 645119 |
| High | admin | logged-in-admin | mobile | /vi/admin/gamification | 32 | 0 | 30 | 0 | 2965 | 504 | 836 | 0.0 | 0.0000 | 0 | 0 | 697628 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha/history | 32 | 2 | 28 | 0 | 2879 | 472 | 1140 | 0.0 | 0.0000 | 0 | 0 | 725538 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | 31 | 1 | 28 | 0 | 2869 | 520 | 1212 | 0.0 | 0.0041 | 0 | 0 | 644840 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha | 31 | 1 | 28 | 0 | 2828 | 496 | 1184 | 0.0 | 0.0041 | 0 | 0 | 725500 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha/history | 31 | 1 | 28 | 0 | 2893 | 488 | 1200 | 0.0 | 0.0041 | 0 | 0 | 726583 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile | 31 | 2 | 27 | 0 | 3100 | 560 | 972 | 0.0 | 0.0538 | 0 | 0 | 635630 |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/leaderboard | 31 | 1 | 28 | 0 | 3204 | 540 | 956 | 0.0 | 0.0041 | 0 | 0 | 650893 |
| High | admin | logged-in-admin | desktop | /vi/admin/disputes | 31 | 1 | 28 | 0 | 2931 | 552 | 944 | 0.0 | 0.0000 | 0 | 0 | 648518 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha/history | 31 | 1 | 28 | 0 | 2858 | 488 | 1240 | 0.0 | 0.0039 | 0 | 0 | 724525 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/inventory | 31 | 1 | 28 | 0 | 2891 | 488 | 1124 | 0.0 | 0.0000 | 0 | 0 | 644668 |
| High | reader-chat | logged-in-admin | desktop | /vi/chat | 30 | 2 | 26 | 0 | 3305 | 528 | 1384 | 0.0 | 0.0041 | 0 | 0 | 632742 |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | 30 | 1 | 27 | 0 | 3291 | 528 | 936 | 0.0 | 0.0279 | 0 | 0 | 642900 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet | 30 | 2 | 26 | 0 | 2876 | 512 | 1220 | 0.0 | 0.0041 | 0 | 0 | 635351 |
| High | admin | logged-in-admin | desktop | /vi/admin/system-configs | 30 | 0 | 28 | 0 | 3023 | 616 | 1172 | 0.0 | 0.0000 | 0 | 0 | 688896 |
| High | admin | logged-in-admin | desktop | /vi/admin/users | 30 | 0 | 28 | 0 | 2896 | 580 | 1276 | 0.0 | 0.0000 | 0 | 0 | 649451 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/inventory | 30 | 0 | 28 | 0 | 2836 | 492 | 1168 | 0.0 | 0.0039 | 0 | 0 | 644125 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha | 30 | 0 | 28 | 0 | 2875 | 548 | 1260 | 0.0 | 0.0039 | 0 | 0 | 724739 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile | 30 | 1 | 27 | 0 | 3072 | 540 | 1208 | 0.0 | 0.0726 | 0 | 0 | 634846 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/leaderboard | 30 | 0 | 28 | 0 | 2802 | 476 | 940 | 0.0 | 0.0039 | 0 | 0 | 650079 |
| High | auth-public | logged-out | mobile | /vi | 30 | 1 | 27 | 0 | 3529 | 944 | 1304 | 0.0 | 0.0000 | 0 | 0 | 602181 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/inventory | 30 | 0 | 28 | 0 | 2925 | 524 | 1164 | 0.0 | 0.0000 | 0 | 0 | 644045 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha | 30 | 0 | 28 | 0 | 2894 | 464 | 1120 | 0.0 | 0.0000 | 0 | 0 | 724275 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha/history | 30 | 0 | 28 | 0 | 2929 | 492 | 880 | 0.0 | 0.0000 | 0 | 0 | 725578 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile | 30 | 1 | 27 | 0 | 3039 | 492 | 828 | 0.0 | 0.0000 | 0 | 0 | 634264 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/leaderboard | 30 | 0 | 28 | 0 | 3246 | 944 | 944 | 0.0 | 0.0000 | 0 | 0 | 649877 |
| High | admin | logged-in-admin | mobile | /vi/admin/system-configs | 30 | 0 | 28 | 0 | 2890 | 444 | 804 | 0.0 | 0.0000 | 0 | 0 | 688709 |
| High | admin | logged-in-admin | mobile | /vi/admin/users | 30 | 0 | 28 | 0 | 2922 | 504 | 1172 | 0.0 | 0.0000 | 0 | 0 | 649439 |
| High | reading | logged-in-reader | mobile | /vi/reading | 30 | 2 | 26 | 0 | 2872 | 444 | 828 | 0.0 | 0.0000 | 0 | 0 | 643174 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile | 30 | 1 | 27 | 0 | 3037 | 456 | 1092 | 0.0 | 0.0821 | 0 | 0 | 634800 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers | 30 | 2 | 26 | 0 | 2918 | 500 | 1132 | 0.0 | 0.0000 | 0 | 0 | 635251 |
| High | community-leaderboard-quest | logged-in-reader | mobile | /vi/leaderboard | 30 | 0 | 28 | 0 | 2849 | 508 | 856 | 0.0 | 0.0000 | 0 | 0 | 650092 |
| High | community-leaderboard-quest | logged-in-reader | mobile | /vi/gamification | 30 | 1 | 27 | 0 | 2881 | 476 | 868 | 0.0 | 0.0000 | 0 | 0 | 643161 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/withdraw | 30 | 2 | 26 | 0 | 2947 | 504 | 1144 | 0.0 | 0.0330 | 0 | 0 | 634276 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/notifications | 30 | 2 | 26 | 0 | 2888 | 480 | 824 | 0.0 | 0.0000 | 0 | 0 | 633513 |
| High | auth-public | logged-out | desktop | /vi | 29 | 0 | 27 | 0 | 4065 | 1516 | 1516 | 522.0 | 0.0000 | 0 | 0 | 600939 |
| High | reading | logged-in-admin | desktop | /vi/reading | 29 | 1 | 26 | 0 | 2893 | 520 | 976 | 0.0 | 0.0041 | 0 | 0 | 642295 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | 29 | 0 | 27 | 0 | 6337 | 540 | 540 | 0.0 | 0.0042 | 0 | 0 | 642810 |
| High | admin | logged-in-admin | desktop | /vi/admin | 29 | 0 | 27 | 0 | 3195 | 532 | 916 | 0.0 | 0.0000 | 0 | 0 | 647051 |
| High | admin | logged-in-admin | desktop | /vi/admin/deposits | 29 | 0 | 27 | 0 | 2727 | 524 | 964 | 0.0 | 0.0000 | 0 | 0 | 647130 |
| High | admin | logged-in-admin | desktop | /vi/admin/reader-requests | 29 | 0 | 27 | 0 | 2832 | 496 | 880 | 0.0 | 0.0000 | 0 | 0 | 645778 |
| High | admin | logged-in-admin | desktop | /vi/admin/readings | 29 | 0 | 27 | 0 | 2720 | 548 | 952 | 0.0 | 0.0000 | 0 | 0 | 648191 |
| High | admin | logged-in-admin | desktop | /vi/admin/withdrawals | 29 | 0 | 27 | 0 | 2857 | 536 | 868 | 0.0 | 0.0000 | 0 | 0 | 645647 |
| High | reading | logged-in-reader | desktop | /vi/reading | 29 | 1 | 26 | 0 | 2869 | 504 | 912 | 0.0 | 0.0039 | 0 | 0 | 642549 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | 29 | 0 | 27 | 0 | 6230 | 496 | 496 | 0.0 | 0.0040 | 0 | 0 | 641708 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/gamification | 29 | 0 | 27 | 0 | 2847 | 660 | 1092 | 0.0 | 0.0277 | 0 | 0 | 642356 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet | 29 | 1 | 26 | 0 | 2854 | 488 | 1140 | 0.0 | 0.0000 | 0 | 0 | 634450 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/withdraw | 29 | 1 | 26 | 0 | 2824 | 616 | 1020 | 0.0 | 0.0095 | 0 | 0 | 633709 |
| High | reading | logged-in-admin | mobile | /vi/reading | 29 | 1 | 26 | 0 | 2918 | 524 | 884 | 0.0 | 0.0000 | 0 | 0 | 642052 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | 29 | 0 | 27 | 0 | 5771 | 484 | 820 | 0.0 | 0.0000 | 0 | 0 | 642629 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers | 29 | 1 | 26 | 0 | 2984 | 524 | 1168 | 0.0 | 0.0000 | 0 | 0 | 634099 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit | 29 | 1 | 26 | 0 | 2880 | 496 | 840 | 0.0 | 0.0000 | 0 | 0 | 631848 |
| High | admin | logged-in-admin | mobile | /vi/admin | 29 | 0 | 27 | 0 | 2791 | 468 | 796 | 0.0 | 0.0000 | 0 | 0 | 646917 |
| High | admin | logged-in-admin | mobile | /vi/admin/deposits | 29 | 0 | 27 | 0 | 2752 | 472 | 796 | 0.0 | 0.0000 | 0 | 0 | 646866 |
| High | admin | logged-in-admin | mobile | /vi/admin/disputes | 29 | 0 | 27 | 0 | 2808 | 472 | 780 | 0.0 | 0.0000 | 0 | 0 | 645411 |
| High | admin | logged-in-admin | mobile | /vi/admin/reader-requests | 29 | 0 | 27 | 0 | 2791 | 476 | 796 | 0.0 | 0.0000 | 0 | 0 | 645897 |
| High | admin | logged-in-admin | mobile | /vi/admin/readings | 29 | 0 | 27 | 0 | 2806 | 516 | 868 | 0.0 | 0.0000 | 0 | 0 | 648308 |
| High | admin | logged-in-admin | mobile | /vi/admin/withdrawals | 29 | 0 | 27 | 0 | 2922 | 544 | 880 | 0.0 | 0.0000 | 0 | 0 | 645575 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 29 | 1 | 26 | 0 | 2992 | 596 | 948 | 0.0 | 0.0000 | 0 | 0 | 631529 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | 29 | 0 | 27 | 0 | 5585 | 456 | 456 | 0.0 | 0.0000 | 0 | 0 | 641802 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet | 29 | 1 | 26 | 0 | 2874 | 448 | 1100 | 0.0 | 0.0000 | 0 | 0 | 634508 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 29 | 1 | 26 | 0 | 2904 | 524 | 852 | 0.0 | 0.0000 | 0 | 0 | 631481 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | 28 | 0 | 26 | 0 | 2794 | 572 | 948 | 0.0 | 0.0041 | 0 | 0 | 630926 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | 28 | 0 | 26 | 0 | 2771 | 516 | 1172 | 0.0 | 0.0489 | 0 | 0 | 630222 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers | 28 | 0 | 26 | 0 | 2895 | 580 | 960 | 0.0 | 0.0041 | 0 | 0 | 633381 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 2754 | 512 | 924 | 0.0 | 0.0041 | 0 | 0 | 630978 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 2895 | 544 | 1132 | 0.0 | 0.0041 | 0 | 0 | 631851 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | 28 | 0 | 26 | 0 | 2706 | 504 | 1208 | 0.0 | 0.0041 | 0 | 0 | 630677 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/notifications | 28 | 0 | 26 | 0 | 2726 | 508 | 892 | 0.0 | 0.0046 | 0 | 0 | 631340 |
| High | reader-chat | logged-in-admin | desktop | /vi/reader/apply | 28 | 0 | 26 | 0 | 2728 | 508 | 924 | 0.0 | 0.0041 | 0 | 0 | 632081 |
| High | reading | logged-in-admin | desktop | /vi/reading/history | 28 | 0 | 26 | 0 | 2726 | 492 | 1084 | 0.0 | 0.0041 | 0 | 0 | 632180 |
| High | admin | logged-in-admin | desktop | /vi/admin/promotions | 28 | 0 | 26 | 0 | 2826 | 512 | 900 | 0.0 | 0.0000 | 0 | 0 | 644114 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/174febce-76ad-4bac-bfd9-7471984a6b35 | 28 | 0 | 26 | 0 | 2716 | 516 | 908 | 0.0 | 0.0041 | 0 | 0 | 631685 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/44f456bd-915f-4f30-ba10-7b3bac6cf8f5 | 28 | 0 | 26 | 0 | 2704 | 532 | 944 | 0.0 | 0.0041 | 0 | 0 | 631719 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/8bf24139-8c59-4fa5-bbdb-477161895ddb | 28 | 0 | 26 | 0 | 2674 | 476 | 868 | 0.0 | 0.0041 | 0 | 0 | 631550 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 2724 | 468 | 872 | 0.0 | 0.0041 | 0 | 0 | 630732 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 2729 | 504 | 892 | 0.0 | 0.0041 | 0 | 0 | 630692 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 2789 | 456 | 844 | 0.0 | 0.0041 | 0 | 0 | 630548 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 2786 | 544 | 1080 | 0.0 | 0.0041 | 0 | 0 | 632425 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 2668 | 480 | 1024 | 0.0 | 0.0041 | 0 | 0 | 632566 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 2722 | 488 | 1060 | 0.0 | 0.0041 | 0 | 0 | 632530 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/mfa | 28 | 0 | 26 | 0 | 2788 | 604 | 984 | 0.0 | 0.0039 | 0 | 0 | 631436 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/reader | 28 | 0 | 26 | 0 | 2811 | 516 | 1268 | 0.0 | 0.0039 | 0 | 0 | 631942 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers | 28 | 0 | 26 | 0 | 2819 | 712 | 1116 | 0.0 | 0.0039 | 0 | 0 | 633478 |
| High | reader-chat | logged-in-reader | desktop | /vi/chat | 28 | 0 | 26 | 0 | 2758 | 696 | 1080 | 0.0 | 0.0039 | 0 | 0 | 631448 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 2785 | 492 | 928 | 0.0 | 0.0039 | 0 | 0 | 631219 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 2707 | 528 | 944 | 0.0 | 0.0039 | 0 | 0 | 631143 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/notifications | 28 | 0 | 26 | 0 | 2843 | 524 | 936 | 0.0 | 0.0044 | 0 | 0 | 631872 |
| High | reader-chat | logged-in-reader | desktop | /vi/reader/apply | 28 | 0 | 26 | 0 | 2723 | 504 | 872 | 0.0 | 0.0039 | 0 | 0 | 632215 |
| High | reading | logged-in-reader | desktop | /vi/reading/history | 28 | 0 | 26 | 0 | 2709 | 496 | 1136 | 0.0 | 0.0039 | 0 | 0 | 632747 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/e8442542-d814-4e53-9ec0-89cda66adb6d | 28 | 0 | 26 | 0 | 2761 | 524 | 900 | 0.0 | 0.0039 | 0 | 0 | 632102 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/0f340ee9-5a42-4a05-b944-f39173ddbdfb | 28 | 0 | 26 | 0 | 2727 | 536 | 904 | 0.0 | 0.0039 | 0 | 0 | 632045 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/f6584958-4d5d-4678-9381-0f5886df197f | 28 | 0 | 26 | 0 | 2816 | 584 | 984 | 0.0 | 0.0039 | 0 | 0 | 631768 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 2775 | 668 | 992 | 0.0 | 0.0039 | 0 | 0 | 631047 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 2702 | 520 | 900 | 0.0 | 0.0039 | 0 | 0 | 630807 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 2794 | 700 | 1084 | 0.0 | 0.0039 | 0 | 0 | 630959 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 2690 | 524 | 1108 | 0.0 | 0.0039 | 0 | 0 | 632810 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 2856 | 588 | 1244 | 0.0 | 0.0039 | 0 | 0 | 632670 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 2908 | 828 | 1452 | 0.0 | 0.0039 | 0 | 0 | 632758 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/mfa | 28 | 0 | 26 | 0 | 2768 | 636 | 964 | 0.0 | 0.0000 | 0 | 0 | 631057 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | 28 | 0 | 26 | 0 | 2824 | 656 | 1016 | 0.0 | 0.0760 | 0 | 0 | 630298 |
| High | reader-chat | logged-in-admin | mobile | /vi/chat | 28 | 0 | 26 | 0 | 3898 | 920 | 920 | 0.0 | 0.0071 | 0 | 0 | 631161 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet | 28 | 0 | 26 | 0 | 3476 | 936 | 1288 | 0.0 | 0.0071 | 0 | 0 | 633785 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 3049 | 852 | 852 | 0.0 | 0.0000 | 0 | 0 | 631643 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/notifications | 28 | 0 | 26 | 0 | 2838 | 576 | 916 | 0.0 | 0.0000 | 0 | 0 | 631632 |
| High | reader-chat | logged-in-admin | mobile | /vi/reader/apply | 28 | 0 | 26 | 0 | 2759 | 484 | 812 | 0.0 | 0.0000 | 0 | 0 | 632088 |
| High | reading | logged-in-admin | mobile | /vi/reading/history | 28 | 0 | 26 | 0 | 2777 | 484 | 820 | 0.0 | 0.0000 | 0 | 0 | 632343 |
| High | admin | logged-in-admin | mobile | /vi/admin/promotions | 28 | 0 | 26 | 0 | 2834 | 504 | 828 | 0.0 | 0.0000 | 0 | 0 | 644143 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/dc1500b6-435d-4af0-92f4-bd3318a2ce9a | 28 | 0 | 26 | 0 | 2754 | 464 | 812 | 0.0 | 0.0000 | 0 | 0 | 631702 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/2eaccbea-6a57-40ee-bd93-314cef2e7685 | 28 | 0 | 26 | 0 | 2772 | 488 | 824 | 0.0 | 0.0000 | 0 | 0 | 631699 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 2743 | 456 | 788 | 0.0 | 0.0000 | 0 | 0 | 630703 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 2845 | 484 | 836 | 0.0 | 0.0000 | 0 | 0 | 630634 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 2918 | 524 | 888 | 0.0 | 0.0000 | 0 | 0 | 632243 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 2857 | 496 | 928 | 0.0 | 0.0000 | 0 | 0 | 632232 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 2863 | 512 | 868 | 0.0 | 0.0000 | 0 | 0 | 632744 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/mfa | 28 | 0 | 26 | 0 | 2819 | 464 | 804 | 0.0 | 0.0000 | 0 | 0 | 631325 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/reader | 28 | 0 | 26 | 0 | 2814 | 616 | 964 | 0.0 | 0.0000 | 0 | 0 | 631917 |
| High | reader-chat | logged-in-reader | mobile | /vi/chat | 28 | 0 | 26 | 0 | 2754 | 500 | 824 | 0.0 | 0.0000 | 0 | 0 | 631391 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 2850 | 468 | 832 | 0.0 | 0.0000 | 0 | 0 | 631043 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 2847 | 476 | 828 | 0.0 | 0.0000 | 0 | 0 | 631472 |
| High | reader-chat | logged-in-reader | mobile | /vi/reader/apply | 28 | 0 | 26 | 0 | 2816 | 460 | 796 | 0.0 | 0.0000 | 0 | 0 | 632324 |
| High | reading | logged-in-reader | mobile | /vi/reading/history | 28 | 0 | 26 | 0 | 2812 | 456 | 808 | 0.0 | 0.0000 | 0 | 0 | 632689 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/173188c4-b981-4fe0-a3d4-0c0fe9946027 | 28 | 0 | 26 | 0 | 2763 | 468 | 800 | 0.0 | 0.0000 | 0 | 0 | 631995 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 2828 | 484 | 812 | 0.0 | 0.0000 | 0 | 0 | 630584 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 2804 | 456 | 780 | 0.0 | 0.0000 | 0 | 0 | 631070 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 2839 | 480 | 828 | 0.0 | 0.0000 | 0 | 0 | 632328 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 2773 | 492 | 848 | 0.0 | 0.0000 | 0 | 0 | 632390 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 2803 | 452 | 800 | 0.0 | 0.0000 | 0 | 0 | 632717 |
| High | auth-public | logged-in-admin | desktop | /vi | 26 | 0 | 24 | 0 | 2695 | 624 | 1192 | 228.0 | 0.0035 | 0 | 0 | 537021 |
| High | auth-public | logged-in-reader | desktop | /vi | 26 | 0 | 24 | 0 | 2765 | 544 | 1128 | 262.0 | 0.0033 | 0 | 0 | 537108 |
| High | auth-public | logged-in-reader | mobile | /vi | 26 | 0 | 24 | 0 | 2878 | 640 | 1008 | 0.0 | 0.0028 | 0 | 0 | 537130 |
| High | auth-public | logged-out | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 2963 | 700 | 700 | 0.0 | 0.0000 | 0 | 0 | 525324 |
| High | auth-public | logged-out | desktop | /vi/login | 24 | 0 | 22 | 0 | 2961 | 744 | 744 | 0.0 | 0.0000 | 0 | 0 | 511635 |
| Medium | auth-public | logged-out | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 2635 | 436 | 768 | 0.0 | 0.0000 | 0 | 0 | 525325 |
| Medium | auth-public | logged-out | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2682 | 472 | 472 | 0.0 | 0.0000 | 0 | 0 | 525322 |
| Medium | auth-public | logged-out | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2812 | 480 | 804 | 0.0 | 0.0000 | 0 | 0 | 525494 |
| Medium | auth-public | logged-in-admin | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 2686 | 504 | 872 | 0.0 | 0.0020 | 0 | 0 | 525604 |
| Medium | auth-public | logged-in-admin | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2661 | 480 | 832 | 0.0 | 0.0020 | 0 | 0 | 525723 |
| Medium | auth-public | logged-in-admin | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2618 | 488 | 816 | 0.0 | 0.0020 | 0 | 0 | 525706 |
| Medium | auth-public | logged-in-reader | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 2704 | 520 | 828 | 0.0 | 0.0019 | 0 | 0 | 525768 |
| Medium | auth-public | logged-in-reader | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2755 | 476 | 860 | 0.0 | 0.0019 | 0 | 0 | 525614 |
| Medium | auth-public | logged-in-reader | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2675 | 544 | 840 | 0.0 | 0.0019 | 0 | 0 | 525740 |
| Medium | auth-public | logged-out | mobile | /vi/register | 25 | 1 | 22 | 0 | 2662 | 468 | 468 | 0.0 | 0.0000 | 0 | 0 | 512847 |
| Medium | auth-public | logged-out | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2723 | 456 | 772 | 0.0 | 0.0000 | 0 | 0 | 525263 |
| Medium | auth-public | logged-out | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2824 | 488 | 796 | 0.0 | 0.0000 | 0 | 0 | 525386 |
| Medium | auth-public | logged-in-admin | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 2753 | 496 | 828 | 0.0 | 0.0032 | 0 | 0 | 525660 |
| Medium | auth-public | logged-in-admin | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2786 | 492 | 800 | 0.0 | 0.0032 | 0 | 0 | 525581 |
| Medium | auth-public | logged-in-admin | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2746 | 516 | 852 | 0.0 | 0.0032 | 0 | 0 | 525571 |
| Medium | auth-public | logged-in-reader | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 2775 | 472 | 800 | 0.0 | 0.0028 | 0 | 0 | 525568 |
| Medium | auth-public | logged-in-reader | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2698 | 440 | 748 | 0.0 | 0.0028 | 0 | 0 | 525719 |
| Medium | auth-public | logged-in-reader | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2732 | 460 | 784 | 0.0 | 0.0028 | 0 | 0 | 525753 |
| Medium | auth-public | logged-out | desktop | /vi/register | 24 | 0 | 22 | 0 | 2703 | 512 | 512 | 0.0 | 0.0000 | 0 | 0 | 512233 |
| Medium | auth-public | logged-out | desktop | /vi/forgot-password | 24 | 0 | 22 | 0 | 2672 | 524 | 524 | 0.0 | 0.0000 | 0 | 0 | 511270 |
| Medium | auth-public | logged-out | desktop | /vi/reset-password | 24 | 0 | 22 | 0 | 2713 | 480 | 480 | 0.0 | 0.0000 | 0 | 0 | 511522 |
| Medium | auth-public | logged-out | desktop | /vi/verify-email | 24 | 0 | 22 | 0 | 2727 | 496 | 496 | 0.0 | 0.0000 | 0 | 0 | 511488 |
| Medium | auth-public | logged-out | mobile | /vi/login | 24 | 0 | 22 | 0 | 2695 | 568 | 568 | 0.0 | 0.0000 | 0 | 0 | 511746 |
| Medium | auth-public | logged-out | mobile | /vi/forgot-password | 24 | 0 | 22 | 0 | 2691 | 468 | 468 | 0.0 | 0.0000 | 0 | 0 | 511292 |
| Medium | auth-public | logged-out | mobile | /vi/reset-password | 24 | 0 | 22 | 0 | 2837 | 604 | 604 | 0.0 | 0.0000 | 0 | 0 | 511406 |
| Medium | auth-public | logged-out | mobile | /vi/verify-email | 24 | 0 | 22 | 0 | 2785 | 524 | 524 | 0.0 | 0.0000 | 0 | 0 | 511408 |

## Major Issues Found

- Critical: 2 page(s) có request count >35, pending request, failed request, hoặc issue nghiêm trọng.
- High: 144 page(s) vượt ngưỡng >25 requests hoặc có request >800ms.
- Medium: 26 page(s) có request trong dải 400-800ms.
- Duplicate: 28 nhóm duplicate request cần kiểm tra over-fetch/cache key.
- Pending: chưa phát hiện pending request bất thường.

### Slow Requests

| Severity | Feature | Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| Critical | reader-chat | logged-in-admin | mobile | /vi/chat | GET | 200 | 1303 | 187 | static | https://www.tarotnow.xyz/_next/static/chunks/023q4p3fs49f5.js |
| Critical | auth-public | logged-out | desktop | /vi | GET | 200 | 1270 | 587 | html | https://www.tarotnow.xyz/vi |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/community | GET | 200 | 1239 | 777 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/chat | GET | 200 | 1222 | 330 | html | https://www.tarotnow.xyz/vi/chat |
| Critical | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | GET | 200 | 1221 | 366 | html | https://www.tarotnow.xyz/vi/gamification |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | GET | 200 | 1204 | 1198 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Frare_title_lucky_star_50_20260416_165453.avif&w=48&q=75 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/chat | GET | 200 | 1159 | 189 | static | https://www.tarotnow.xyz/_next/static/chunks/134bl8i0js1.i.js |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1137 | 233 | third-party | https://img.tarotnow.xyz/light-god-50/08_The_Chariot_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | GET | 200 | 1123 | 1116 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Fexp_booster_50_20260416_165452.avif&w=48&q=75 |
| Critical | community-leaderboard-quest | logged-in-admin | mobile | /vi/community | GET | 200 | 1111 | 593 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1200&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | GET | 200 | 1110 | 1104 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Fpower_booster_50_20260416_165453.avif&w=48&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | GET | 200 | 1108 | 1100 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Ffree_draw_ticket_50_20260416_165452.avif&w=48&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | GET | 200 | 1108 | 1099 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Fdefense_booster_50_20260416_165452.avif&w=48&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1101 | 254 | third-party | https://img.tarotnow.xyz/light-god-50/07_The_Lovers_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | GET | 200 | 1065 | 389 | html | https://www.tarotnow.xyz/vi/wallet |
| Critical | auth-public | logged-out | desktop | /vi | GET | 200 | 1056 | 571 | static | https://www.tarotnow.xyz/_next/static/chunks/023q4p3fs49f5.js |
| Critical | reader-chat | logged-in-admin | mobile | /vi/chat | GET | 200 | 1041 | 185 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| Critical | auth-public | logged-out | mobile | /vi | GET | 200 | 1017 | 635 | html | https://www.tarotnow.xyz/vi |
| Critical | auth-public | logged-out | desktop | /vi | GET | 200 | 991 | 585 | static | https://www.tarotnow.xyz/_next/static/chunks/0eqe0oax2~t9t.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet | GET | 200 | 941 | 379 | static | https://www.tarotnow.xyz/_next/static/chunks/023q4p3fs49f5.js |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 937 | 188 | third-party | https://img.tarotnow.xyz/light-god-50/08_The_Chariot_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet | GET | 200 | 928 | 380 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 912 | 303 | third-party | https://img.tarotnow.xyz/light-god-50/07_The_Lovers_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha | GET | 200 | 906 | 678 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Frare_title_lucky_star_50_20260416_165453.avif&w=256&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 906 | 116 | third-party | https://img.tarotnow.xyz/light-god-50/06_The_Hierophant_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | auth-public | logged-out | desktop | /vi/login | GET | 200 | 894 | 573 | html | https://www.tarotnow.xyz/vi/login |
| Critical | auth-public | logged-out | mobile | /vi/legal/tos | GET | 200 | 894 | 547 | html | https://www.tarotnow.xyz/vi/legal/tos |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 884 | 147 | third-party | https://img.tarotnow.xyz/light-god-50/06_The_Hierophant_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet | GET | 200 | 884 | 378 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 882 | 381 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet | GET | 200 | 881 | 475 | html | https://www.tarotnow.xyz/vi/wallet |
| Critical | reader-chat | logged-in-admin | mobile | /vi/chat | GET | 200 | 872 | 183 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet | GET | 200 | 871 | 407 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| Critical | admin | logged-in-admin | desktop | /vi/admin/system-configs | GET | 200 | 870 | 368 | html | https://www.tarotnow.xyz/vi/admin/system-configs |
| Critical | reader-chat | logged-in-admin | mobile | /vi/chat | GET | 200 | 869 | 860 | static | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers | GET | 200 | 868 | 334 | html | https://www.tarotnow.xyz/vi/readers |
| Critical | community-leaderboard-quest | logged-in-admin | mobile | /vi/community | GET | 200 | 865 | 347 | html | https://www.tarotnow.xyz/vi/community |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers | GET | 200 | 857 | 352 | html | https://www.tarotnow.xyz/vi/readers |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/withdraw | GET | 200 | 849 | 330 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| Critical | admin | logged-in-admin | desktop | /vi/admin/users | GET | 200 | 845 | 386 | html | https://www.tarotnow.xyz/vi/admin/users |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/inventory | GET | 200 | 837 | 339 | html | https://www.tarotnow.xyz/vi/inventory |
| Critical | admin | logged-in-admin | mobile | /vi/admin/withdrawals | GET | 200 | 829 | 333 | html | https://www.tarotnow.xyz/vi/admin/withdrawals |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha/history | GET | 200 | 827 | 338 | html | https://www.tarotnow.xyz/vi/gacha/history |
| Critical | admin | logged-in-admin | mobile | /vi/admin/users | GET | 200 | 827 | 337 | html | https://www.tarotnow.xyz/vi/admin/users |
| Critical | reading | logged-in-admin | mobile | /vi/reading | GET | 200 | 826 | 376 | html | https://www.tarotnow.xyz/vi/reading |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/notifications | GET | 200 | 825 | 324 | html | https://www.tarotnow.xyz/vi/notifications |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 819 | 343 | html | https://www.tarotnow.xyz/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet | GET | 200 | 817 | 329 | html | https://www.tarotnow.xyz/vi/wallet |
| Critical | admin | logged-in-admin | desktop | /vi/admin | GET | 200 | 814 | 553 | static | https://www.tarotnow.xyz/_next/static/chunks/0uvdfew0zvv3h.js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha/history | GET | 200 | 812 | 329 | html | https://www.tarotnow.xyz/vi/gacha/history |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile | GET | 200 | 811 | 325 | html | https://www.tarotnow.xyz/vi/profile |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/gamification | GET | 200 | 811 | 333 | html | https://www.tarotnow.xyz/vi/gamification |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/inventory | GET | 200 | 810 | 330 | html | https://www.tarotnow.xyz/vi/inventory |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | GET | 200 | 806 | 294 | static | https://www.tarotnow.xyz/_next/static/chunks/023q4p3fs49f5.js |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha/history | GET | 200 | 805 | 322 | html | https://www.tarotnow.xyz/vi/gacha/history |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | GET | 200 | 804 | 328 | html | https://www.tarotnow.xyz/vi/inventory |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | GET | 200 | 804 | 334 | html | https://www.tarotnow.xyz/vi/profile |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 804 | 317 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| Medium | community-leaderboard-quest | logged-in-reader | desktop | /vi/gamification | GET | 200 | 797 | 322 | html | https://www.tarotnow.xyz/vi/gamification |
| Medium | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 796 | 116 | third-party | https://img.tarotnow.xyz/light-god-50/05_The_Emperor_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| Medium | auth-public | logged-in-reader | mobile | /vi | GET | 200 | 794 | 451 | html | https://www.tarotnow.xyz/vi |
| Medium | admin | logged-in-admin | desktop | /vi/admin/disputes | GET | 200 | 793 | 329 | html | https://www.tarotnow.xyz/vi/admin/disputes |
| Medium | admin | logged-in-admin | mobile | /vi/admin/gamification | GET | 200 | 790 | 327 | html | https://www.tarotnow.xyz/vi/admin/gamification |
| Medium | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | GET | 200 | 789 | 323 | html | https://www.tarotnow.xyz/vi/wallet |
| Medium | admin | logged-in-admin | desktop | /vi/admin | GET | 200 | 789 | 543 | static | https://www.tarotnow.xyz/_next/static/chunks/0xl.ndi-x969l.js |
| Medium | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha | GET | 200 | 788 | 303 | html | https://www.tarotnow.xyz/vi/gacha |
| Medium | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | GET | 200 | 788 | 292 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| Medium | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha | GET | 200 | 788 | 310 | html | https://www.tarotnow.xyz/vi/gacha |
| Medium | reader-chat | logged-in-admin | desktop | /vi/readers | GET | 200 | 786 | 351 | html | https://www.tarotnow.xyz/vi/readers |
| Medium | admin | logged-in-admin | desktop | /vi/admin/withdrawals | GET | 200 | 785 | 316 | html | https://www.tarotnow.xyz/vi/admin/withdrawals |
| Medium | reading | logged-in-reader | mobile | /vi/reading/session/70126a07-9e85-4898-8b64-061f6d14e550 | GET | 200 | 785 | 334 | html | https://www.tarotnow.xyz/vi/reading/session/70126a07-9e85-4898-8b64-061f6d14e550 |
| Medium | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha | GET | 200 | 784 | 322 | html | https://www.tarotnow.xyz/vi/gacha |
| Medium | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet | GET | 200 | 782 | 311 | html | https://www.tarotnow.xyz/vi/wallet |
| Medium | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile | GET | 200 | 781 | 352 | html | https://www.tarotnow.xyz/vi/profile |
| Medium | reader-chat | logged-in-reader | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 779 | 330 | html | https://www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| Medium | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet | GET | 200 | 778 | 303 | html | https://www.tarotnow.xyz/vi/wallet |
| Medium | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | GET | 200 | 776 | 314 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| Medium | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit | GET | 200 | 775 | 347 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| Medium | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | GET | 200 | 774 | 316 | html | https://www.tarotnow.xyz/vi/gamification |
| Medium | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha | GET | 200 | 772 | 322 | html | https://www.tarotnow.xyz/vi/gacha |

### Duplicate API / Request Candidates

| Severity | Feature | Scenario | Viewport | Route | Count | Request Key |
| --- | --- | --- | --- | --- | ---: | --- |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/themes/prismatic-royal.css |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0h38g3weqde1h.js |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0xovby2vf96zv.js |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0d2b1s5.nlpim.js |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/023q4p3fs49f5.js |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0eqe0oax2~t9t.js |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0c4xf8mjx0gok.js |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/134bl8i0js1.i.js |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 2 | GET https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/api/auth/session?mode=lite |

### Pending Requests

| Severity | Feature | Scenario | Viewport | Route | URL |
| --- | --- | --- | --- | --- | --- |
| - | - | - | - | - | - |

## Optimization Plan

1. Fix shared Critical issues first: auth/session loops, layout-level fetch churn, failed requests, pending non-persistent requests.
2. Fix feature Critical/High issues next, starting with the feature that has the most affected page-scenario combinations.
3. Fix duplicate API calls by inspecting query keys, staleTime, refetch triggers, and parent/child component fetch ownership.
4. Fix image/cache issues by checking Next Image usage, remote patterns, dimensions, lazy/eager strategy, and modal reopen behavior.
5. Re-run the affected feature benchmark after every hotspot fix, then run full matrix before final deploy validation.

## Recommended Refactors

- Middleware/session: inspect only if report shows session API churn, handshake redirects, or auth-related duplicate requests.
- TanStack Query: inspect feature hooks whose API endpoints appear in duplicate request candidates.
- App Router layouts: inspect layout/provider boundaries if multiple unrelated features share the same duplicate or slow request pattern.
- Custom hooks: inspect effects only when a route shows repeated interaction or post-load requests.
- Image loading: inspect collection/gacha/community routes when slow static/image requests dominate.
- Route prefetch: inspect Link usage only when benchmark shows route-navigation prefetch churn causing unnecessary requests.

## Final Validation

- Baseline benchmark: pending until run is recorded.
- Feature benchmark after fixes: pending until hotspot is selected.
- Local verification: pending.
- GitHub Actions: pending.
- Post-deploy full production benchmark: pending.
