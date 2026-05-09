# PERFORMANCE AUDIT - TarotNow

## Executive Summary

- Generated at (UTC): 2026-05-09T17:58:09.359Z
- Benchmark generated at (UTC): 2026-05-09T17:57:59.244Z
- Benchmark input: Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark.json
- Base origin: https://www.tarotnow.xyz
- Locale prefix: /vi
- Benchmark mode: full-matrix
- Total scenarios: 6
- Total pages measured: 150
- Critical pages: 2
- High pages: 121
- Medium pages: 27
- Slow requests >800ms: 56
- Slow requests 400-800ms: 496
- Request thresholds: >25 suspicious, >35 severe
- Slow request thresholds: >400ms optimize, >800ms serious

## Scenario Coverage

| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Failed | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 24.9 | 2960 | 0 | 0 | yes |
| logged-in-admin | desktop | 33 | 28.4 | 3191 | 2 | 0 | yes |
| logged-in-reader | desktop | 33 | 28.5 | 3008 | 0 | 0 | yes |
| logged-out | mobile | 9 | 25.0 | 2873 | 0 | 0 | yes |
| logged-in-admin | mobile | 33 | 28.5 | 2975 | 0 | 0 | yes |
| logged-in-reader | mobile | 33 | 28.9 | 2952 | 0 | 0 | yes |

## Route Family Coverage

| Scenario | Viewport | Family | Pages | Avg requests/page | Avg nav (ms) | Pending | Failed |
| --- | --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | auth-public | 3 | 25.0 | 2912 | 0 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | 3 | 30.3 | 3494 | 0 | 0 |
| logged-in-admin | desktop | home | 1 | 29.0 | 3583 | 0 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | 4 | 30.3 | 3967 | 0 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | 8 | 28.4 | 3082 | 2 | 0 |
| logged-in-admin | desktop | reader-chat | 9 | 28.2 | 3018 | 0 | 0 |
| logged-in-admin | desktop | reading | 5 | 28.0 | 2963 | 0 | 0 |
| logged-in-admin | mobile | auth-public | 3 | 25.0 | 2711 | 0 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | 3 | 30.0 | 3124 | 0 | 0 |
| logged-in-admin | mobile | home | 1 | 26.0 | 2767 | 0 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | 4 | 30.3 | 3660 | 0 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | 8 | 28.4 | 2894 | 0 | 0 |
| logged-in-admin | mobile | reader-chat | 9 | 28.0 | 2840 | 0 | 0 |
| logged-in-admin | mobile | reading | 5 | 30.0 | 2911 | 0 | 0 |
| logged-in-reader | desktop | auth-public | 3 | 25.0 | 2755 | 0 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | 3 | 30.0 | 3126 | 0 | 0 |
| logged-in-reader | desktop | home | 1 | 26.0 | 2762 | 0 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | 4 | 31.5 | 3974 | 0 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | 8 | 28.5 | 2907 | 0 | 0 |
| logged-in-reader | desktop | reader-chat | 9 | 28.0 | 2821 | 0 | 0 |
| logged-in-reader | desktop | reading | 5 | 29.0 | 2865 | 0 | 0 |
| logged-in-reader | mobile | auth-public | 3 | 25.0 | 2689 | 0 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | 3 | 31.7 | 3175 | 0 | 0 |
| logged-in-reader | mobile | home | 1 | 26.0 | 2781 | 0 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | 4 | 31.3 | 3631 | 0 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | 8 | 28.5 | 2839 | 0 | 0 |
| logged-in-reader | mobile | reader-chat | 9 | 28.2 | 2814 | 0 | 0 |
| logged-in-reader | mobile | reading | 5 | 30.2 | 2898 | 0 | 0 |
| logged-out | desktop | auth-public | 8 | 24.4 | 2863 | 0 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 3737 | 0 | 0 |
| logged-out | mobile | auth-public | 8 | 24.4 | 2797 | 0 | 0 |
| logged-out | mobile | home | 1 | 30.0 | 3486 | 0 | 0 |

## Owner Summary by Route Family

| Route family | Pages | Requests | Static | Same-site media | API | HTML | Websocket | Telemetry | Third-party | Likely next action |
| --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| reader-chat | 36 | 1012 | 936 | 0 | 4 | 36 | 0 | 36 | 0 | FE chunk/lazy-load review |
| profile-wallet-notifications | 32 | 910 | 836 | 0 | 9 | 32 | 0 | 32 | 0 | FE chunk/lazy-load review |
| auth-public | 28 | 690 | 634 | 0 | 0 | 28 | 0 | 28 | 0 | FE chunk/lazy-load review |
| reading | 20 | 586 | 535 | 0 | 11 | 20 | 0 | 20 | 0 | FE chunk/lazy-load review |
| inventory-gacha-collection | 16 | 493 | 444 | 10 | 7 | 16 | 0 | 16 | 0 | FE chunk/lazy-load review |
| community-leaderboard-quest | 12 | 366 | 335 | 1 | 6 | 12 | 0 | 12 | 0 | FE chunk/lazy-load review |
| home | 6 | 166 | 153 | 0 | 1 | 6 | 0 | 6 | 0 | FE chunk/lazy-load review |

## Detailed Metrics Table

| Severity | Feature | Scenario | Viewport | Route | Requests | API | Static | Same-site media | Third-party | Nav (ms) | FCP (ms) | LCP (ms) | TBT (ms) | CLS | Pending | Failed | Transfer bytes |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | 28 | 0 | 26 | 0 | 0 | 3039 | 860 | 1232 | 0.0 | 0.0041 | 1 | 0 | 630249 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | 28 | 0 | 26 | 0 | 0 | 2787 | 888 | 1504 | 0.0 | 0.0041 | 1 | 0 | 630583 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha | 35 | 0 | 28 | 5 | 0 | 2990 | 548 | 1120 | 0.0 | 0.0037 | 0 | 0 | 1059506 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha | 35 | 0 | 28 | 5 | 0 | 2885 | 516 | 876 | 0.0 | 0.0000 | 0 | 0 | 1059760 |
| High | community-leaderboard-quest | logged-in-reader | mobile | /vi/community | 35 | 1 | 31 | 1 | 0 | 3705 | 480 | 980 | 0.0 | 0.0000 | 0 | 0 | 794807 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/b307c06a-d79a-48d4-a939-73621cc5752f | 33 | 2 | 29 | 0 | 0 | 3013 | 568 | 904 | 0.0 | 0.0000 | 0 | 0 | 692710 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/f954a1be-52f2-4c60-be02-9a3cd82a91b1 | 33 | 2 | 29 | 0 | 0 | 3053 | 488 | 876 | 0.0 | 0.0000 | 0 | 0 | 692810 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/4b81ce2f-a326-4fa1-9742-389e9fb34af3 | 33 | 2 | 29 | 0 | 0 | 2959 | 472 | 852 | 0.0 | 0.0000 | 0 | 0 | 693053 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/81b2db93-97f5-438e-b47f-913ee0895dbc | 33 | 2 | 29 | 0 | 0 | 2998 | 480 | 840 | 0.0 | 0.0000 | 0 | 0 | 693059 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/1c308e30-0171-4814-8f83-bff4919171ee | 32 | 1 | 29 | 0 | 0 | 2964 | 572 | 984 | 0.0 | 0.0040 | 0 | 0 | 713213 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile | 32 | 2 | 27 | 0 | 0 | 3019 | 548 | 892 | 0.0 | 0.0000 | 0 | 0 | 637754 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha/history | 31 | 1 | 28 | 0 | 0 | 3042 | 880 | 1712 | 0.0 | 0.0041 | 0 | 0 | 726607 |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/leaderboard | 31 | 1 | 28 | 0 | 0 | 3497 | 780 | 1196 | 0.0 | 0.0041 | 0 | 0 | 651050 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/inventory | 31 | 1 | 28 | 0 | 0 | 2911 | 512 | 1196 | 0.0 | 0.0037 | 0 | 0 | 644534 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha/history | 31 | 1 | 28 | 0 | 0 | 2900 | 560 | 1404 | 0.0 | 0.0037 | 0 | 0 | 724809 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/inventory | 31 | 1 | 28 | 0 | 0 | 2971 | 476 | 1104 | 0.0 | 0.0000 | 0 | 0 | 644797 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha | 31 | 1 | 28 | 0 | 0 | 2907 | 464 | 1120 | 0.0 | 0.0000 | 0 | 0 | 724870 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha/history | 31 | 1 | 28 | 0 | 0 | 2880 | 512 | 1196 | 0.0 | 0.0000 | 0 | 0 | 724653 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | 30 | 0 | 28 | 0 | 0 | 3026 | 1044 | 1304 | 0.0 | 0.0041 | 0 | 0 | 643919 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha | 30 | 0 | 28 | 0 | 0 | 2892 | 776 | 1212 | 0.0 | 0.0041 | 0 | 0 | 724122 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | 30 | 1 | 27 | 0 | 0 | 6907 | 976 | 976 | 0.0 | 0.0042 | 0 | 0 | 643319 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile | 30 | 1 | 27 | 0 | 0 | 3348 | 660 | 1156 | 0.0 | 0.0489 | 0 | 0 | 634471 |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/community | 30 | 0 | 28 | 0 | 0 | 3587 | 644 | 1292 | 0.0 | 0.0041 | 0 | 0 | 642333 |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | 30 | 1 | 27 | 0 | 0 | 3398 | 896 | 1336 | 0.0 | 0.0279 | 0 | 0 | 643284 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile | 30 | 1 | 27 | 0 | 0 | 3080 | 704 | 1156 | 0.0 | 0.0723 | 0 | 0 | 634643 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/leaderboard | 30 | 0 | 28 | 0 | 0 | 2938 | 528 | 1044 | 0.0 | 0.0037 | 0 | 0 | 650373 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/community | 30 | 0 | 28 | 0 | 0 | 3589 | 824 | 1444 | 0.0 | 0.0037 | 0 | 0 | 642824 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/gamification | 30 | 1 | 27 | 0 | 0 | 2852 | 664 | 1128 | 0.0 | 0.0274 | 0 | 0 | 643480 |
| High | auth-public | logged-out | mobile | /vi | 30 | 1 | 27 | 0 | 0 | 3486 | 964 | 964 | 0.0 | 0.0000 | 0 | 0 | 602237 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha/history | 30 | 0 | 28 | 0 | 0 | 2938 | 456 | 840 | 0.0 | 0.0000 | 0 | 0 | 725573 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile | 30 | 1 | 27 | 0 | 0 | 3078 | 596 | 952 | 0.0 | 0.0000 | 0 | 0 | 635037 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/leaderboard | 30 | 0 | 28 | 0 | 0 | 2881 | 456 | 820 | 0.0 | 0.0000 | 0 | 0 | 649877 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/community | 30 | 0 | 28 | 0 | 0 | 3608 | 752 | 1244 | 0.0 | 0.0000 | 0 | 0 | 642561 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 30 | 1 | 27 | 0 | 0 | 2883 | 536 | 880 | 0.0 | 0.0000 | 0 | 0 | 642987 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/inventory | 30 | 0 | 28 | 0 | 0 | 2905 | 500 | 1188 | 0.0 | 0.0000 | 0 | 0 | 644168 |
| High | community-leaderboard-quest | logged-in-reader | mobile | /vi/leaderboard | 30 | 0 | 28 | 0 | 0 | 2879 | 492 | 852 | 0.0 | 0.0000 | 0 | 0 | 650117 |
| High | community-leaderboard-quest | logged-in-reader | mobile | /vi/gamification | 30 | 1 | 27 | 0 | 0 | 2942 | 488 | 880 | 0.0 | 0.0000 | 0 | 0 | 642870 |
| High | auth-public | logged-out | desktop | /vi | 29 | 0 | 27 | 0 | 0 | 3737 | 1356 | 1356 | 583.0 | 0.0000 | 0 | 0 | 600913 |
| High | auth-public | logged-in-admin | desktop | /vi | 29 | 0 | 27 | 0 | 0 | 3583 | 972 | 1732 | 109.0 | 0.0041 | 0 | 0 | 607284 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit | 29 | 1 | 26 | 0 | 0 | 3094 | 788 | 1180 | 0.0 | 0.0041 | 0 | 0 | 632003 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 29 | 1 | 26 | 0 | 0 | 2995 | 768 | 1440 | 0.0 | 0.0041 | 0 | 0 | 633221 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 29 | 1 | 26 | 0 | 0 | 3024 | 652 | 1408 | 0.0 | 0.0041 | 0 | 0 | 633100 |
| High | reading | logged-in-reader | desktop | /vi/reading | 29 | 1 | 26 | 0 | 0 | 2919 | 520 | 1044 | 0.0 | 0.0037 | 0 | 0 | 642176 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | 29 | 0 | 27 | 0 | 0 | 7096 | 508 | 920 | 5.0 | 0.0037 | 0 | 0 | 641783 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/withdraw | 29 | 1 | 26 | 0 | 0 | 2878 | 512 | 1192 | 0.0 | 0.0092 | 0 | 0 | 633578 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/notifications | 29 | 1 | 26 | 0 | 0 | 2844 | 620 | 1056 | 0.0 | 0.0037 | 0 | 0 | 632906 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | 29 | 0 | 27 | 0 | 0 | 5823 | 536 | 536 | 32.0 | 0.0000 | 0 | 0 | 642755 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 29 | 1 | 26 | 0 | 0 | 2901 | 680 | 1020 | 0.0 | 0.0071 | 0 | 0 | 631130 |
| High | reading | logged-in-reader | mobile | /vi/reading | 29 | 1 | 26 | 0 | 0 | 2925 | 472 | 868 | 0.0 | 0.0000 | 0 | 0 | 641777 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | 29 | 0 | 27 | 0 | 0 | 5853 | 444 | 760 | 0.0 | 0.0000 | 0 | 0 | 641866 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers | 29 | 1 | 26 | 0 | 0 | 2909 | 556 | 892 | 0.0 | 0.0000 | 0 | 0 | 634630 |
| High | reader-chat | logged-in-reader | mobile | /vi/chat | 29 | 1 | 26 | 0 | 0 | 2901 | 480 | 856 | 0.0 | 0.0000 | 0 | 0 | 632129 |
| High | reading | logged-in-admin | desktop | /vi/reading | 28 | 0 | 26 | 0 | 0 | 3037 | 864 | 1292 | 0.0 | 0.0041 | 0 | 0 | 641350 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | 28 | 0 | 26 | 0 | 0 | 3129 | 832 | 1192 | 0.0 | 0.0041 | 0 | 0 | 631097 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers | 28 | 0 | 26 | 0 | 0 | 3378 | 1172 | 1464 | 0.0 | 0.0041 | 0 | 0 | 633461 |
| High | reader-chat | logged-in-admin | desktop | /vi/chat | 28 | 0 | 26 | 0 | 0 | 3089 | 912 | 1416 | 0.0 | 0.0041 | 0 | 0 | 631102 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet | 28 | 0 | 26 | 0 | 0 | 3097 | 700 | 1500 | 0.0 | 0.0041 | 0 | 0 | 633716 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 0 | 2988 | 728 | 1460 | 0.0 | 0.0041 | 0 | 0 | 631913 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/notifications | 28 | 0 | 26 | 0 | 0 | 3170 | 964 | 1368 | 0.0 | 0.0041 | 0 | 0 | 631665 |
| High | reader-chat | logged-in-admin | desktop | /vi/reader/apply | 28 | 0 | 26 | 0 | 0 | 3018 | 684 | 1072 | 0.0 | 0.0041 | 0 | 0 | 632183 |
| High | reading | logged-in-admin | desktop | /vi/reading/history | 28 | 0 | 26 | 0 | 0 | 2905 | 716 | 1336 | 0.0 | 0.0041 | 0 | 0 | 632452 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/4d43e530-faa8-45de-8fd0-aa7c2575c4d5 | 28 | 0 | 26 | 0 | 0 | 2943 | 748 | 1104 | 0.0 | 0.0041 | 0 | 0 | 631603 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/8bf4b9ae-fcba-41af-8462-4163a214cb55 | 28 | 0 | 26 | 0 | 0 | 3047 | 820 | 1212 | 0.0 | 0.0041 | 0 | 0 | 631566 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/23fb1aab-7675-4486-885a-f69d5050984e | 28 | 0 | 26 | 0 | 0 | 2883 | 820 | 1200 | 0.0 | 0.0041 | 0 | 0 | 631854 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 0 | 2948 | 820 | 1200 | 0.0 | 0.0041 | 0 | 0 | 630818 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 0 | 2935 | 968 | 968 | 0.0 | 0.0041 | 0 | 0 | 630785 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 0 | 2922 | 928 | 928 | 0.0 | 0.0041 | 0 | 0 | 630692 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 0 | 2851 | 688 | 1424 | 0.0 | 0.0041 | 0 | 0 | 632469 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/mfa | 28 | 0 | 26 | 0 | 0 | 2799 | 560 | 980 | 0.0 | 0.0037 | 0 | 0 | 631046 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/reader | 28 | 0 | 26 | 0 | 0 | 2841 | 532 | 1344 | 0.0 | 0.0037 | 0 | 0 | 631903 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers | 28 | 0 | 26 | 0 | 0 | 2859 | 628 | 1116 | 0.0 | 0.0037 | 0 | 0 | 633407 |
| High | reader-chat | logged-in-reader | desktop | /vi/chat | 28 | 0 | 26 | 0 | 0 | 2862 | 684 | 1132 | 0.0 | 0.0037 | 0 | 0 | 631400 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet | 28 | 0 | 26 | 0 | 0 | 2915 | 576 | 1292 | 0.0 | 0.0037 | 0 | 0 | 633797 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 0 | 2959 | 560 | 1080 | 0.0 | 0.0037 | 0 | 0 | 631333 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 0 | 2937 | 860 | 1272 | 0.0 | 0.0037 | 0 | 0 | 631433 |
| High | reader-chat | logged-in-reader | desktop | /vi/reader/apply | 28 | 0 | 26 | 0 | 0 | 2713 | 528 | 904 | 0.0 | 0.0037 | 0 | 0 | 631946 |
| High | reading | logged-in-reader | desktop | /vi/reading/history | 28 | 0 | 26 | 0 | 0 | 2857 | 548 | 1152 | 0.0 | 0.0037 | 0 | 0 | 632426 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/f01fe337-50d1-47c7-a39b-94da807fc70c | 28 | 0 | 26 | 0 | 0 | 2778 | 576 | 968 | 0.0 | 0.0037 | 0 | 0 | 631744 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/4100878a-d5b0-4d16-a391-b4453647f29b | 28 | 0 | 26 | 0 | 0 | 2809 | 616 | 1040 | 0.0 | 0.0037 | 0 | 0 | 631828 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 0 | 2785 | 580 | 992 | 0.0 | 0.0037 | 0 | 0 | 630950 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 0 | 2871 | 508 | 1000 | 0.0 | 0.0037 | 0 | 0 | 630714 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 0 | 2838 | 664 | 1068 | 0.0 | 0.0037 | 0 | 0 | 631130 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 0 | 2783 | 632 | 1268 | 0.0 | 0.0037 | 0 | 0 | 632521 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 0 | 2820 | 784 | 1400 | 0.0 | 0.0037 | 0 | 0 | 632683 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 0 | 2860 | 608 | 1236 | 0.0 | 0.0037 | 0 | 0 | 632735 |
| High | reading | logged-in-admin | mobile | /vi/reading | 28 | 0 | 26 | 0 | 0 | 2884 | 712 | 1052 | 0.0 | 0.0000 | 0 | 0 | 641109 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/mfa | 28 | 0 | 26 | 0 | 0 | 2886 | 532 | 904 | 0.0 | 0.0000 | 0 | 0 | 631305 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | 28 | 0 | 26 | 0 | 0 | 3004 | 736 | 1088 | 0.0 | 0.0760 | 0 | 0 | 630329 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers | 28 | 0 | 26 | 0 | 0 | 2912 | 512 | 1144 | 0.0 | 0.0000 | 0 | 0 | 633375 |
| High | reader-chat | logged-in-admin | mobile | /vi/chat | 28 | 0 | 26 | 0 | 0 | 2867 | 700 | 1040 | 0.0 | 0.0000 | 0 | 0 | 631117 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet | 28 | 0 | 26 | 0 | 0 | 2862 | 572 | 908 | 0.0 | 0.0000 | 0 | 0 | 633658 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 0 | 2745 | 472 | 804 | 0.0 | 0.0000 | 0 | 0 | 630994 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 0 | 2947 | 592 | 960 | 0.0 | 0.0000 | 0 | 0 | 631809 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/notifications | 28 | 0 | 26 | 0 | 0 | 2725 | 600 | 948 | 0.0 | 0.0000 | 0 | 0 | 631464 |
| High | reader-chat | logged-in-admin | mobile | /vi/reader/apply | 28 | 0 | 26 | 0 | 0 | 2833 | 568 | 896 | 0.0 | 0.0000 | 0 | 0 | 632069 |
| High | reading | logged-in-admin | mobile | /vi/reading/history | 28 | 0 | 26 | 0 | 0 | 2844 | 620 | 964 | 0.0 | 0.0000 | 0 | 0 | 632218 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/c29d9601-a006-4ed4-8d19-76af37aa2a9f | 28 | 0 | 26 | 0 | 0 | 2761 | 468 | 804 | 0.0 | 0.0000 | 0 | 0 | 631591 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 0 | 2763 | 608 | 944 | 0.0 | 0.0000 | 0 | 0 | 630897 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 0 | 2805 | 504 | 852 | 0.0 | 0.0000 | 0 | 0 | 630536 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 0 | 2719 | 452 | 784 | 0.0 | 0.0000 | 0 | 0 | 630678 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 0 | 2945 | 688 | 1036 | 0.0 | 0.0000 | 0 | 0 | 632612 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 0 | 2974 | 524 | 944 | 0.0 | 0.0000 | 0 | 0 | 632373 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 0 | 2739 | 568 | 920 | 0.0 | 0.0000 | 0 | 0 | 632797 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/mfa | 28 | 0 | 26 | 0 | 0 | 2723 | 536 | 892 | 0.0 | 0.0000 | 0 | 0 | 631030 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/reader | 28 | 0 | 26 | 0 | 0 | 2842 | 644 | 988 | 0.0 | 0.0000 | 0 | 0 | 631886 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet | 28 | 0 | 26 | 0 | 0 | 2960 | 560 | 1232 | 0.0 | 0.0000 | 0 | 0 | 633887 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 0 | 2764 | 464 | 812 | 0.0 | 0.0000 | 0 | 0 | 631071 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 0 | 2729 | 460 | 792 | 0.0 | 0.0000 | 0 | 0 | 631214 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/withdraw | 28 | 0 | 26 | 0 | 0 | 2835 | 532 | 888 | 0.0 | 0.0330 | 0 | 0 | 632670 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/notifications | 28 | 0 | 26 | 0 | 0 | 2836 | 568 | 912 | 0.0 | 0.0000 | 0 | 0 | 631686 |
| High | reader-chat | logged-in-reader | mobile | /vi/reader/apply | 28 | 0 | 26 | 0 | 0 | 2788 | 492 | 828 | 0.0 | 0.0000 | 0 | 0 | 632017 |
| High | reading | logged-in-reader | mobile | /vi/reading/history | 28 | 0 | 26 | 0 | 0 | 2770 | 580 | 932 | 0.0 | 0.0000 | 0 | 0 | 632273 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/b2c27c49-ed23-482e-a97a-58bd1e3f893b | 28 | 0 | 26 | 0 | 0 | 2840 | 560 | 900 | 0.0 | 0.0000 | 0 | 0 | 631929 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 0 | 2851 | 496 | 828 | 0.0 | 0.0000 | 0 | 0 | 630782 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 0 | 2899 | 504 | 892 | 0.0 | 0.0000 | 0 | 0 | 630828 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 0 | 2715 | 456 | 796 | 0.0 | 0.0000 | 0 | 0 | 630712 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 0 | 2710 | 464 | 812 | 0.0 | 0.0000 | 0 | 0 | 632566 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 0 | 2744 | 460 | 800 | 0.0 | 0.0000 | 0 | 0 | 632618 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 0 | 2807 | 440 | 904 | 0.0 | 0.0000 | 0 | 0 | 632854 |
| High | auth-public | logged-in-reader | desktop | /vi | 26 | 0 | 24 | 0 | 0 | 2762 | 604 | 1352 | 715.0 | 0.0033 | 0 | 0 | 537159 |
| High | auth-public | logged-in-admin | mobile | /vi | 26 | 0 | 24 | 0 | 0 | 2767 | 664 | 1032 | 0.0 | 0.0032 | 0 | 0 | 537118 |
| High | auth-public | logged-in-reader | mobile | /vi | 26 | 0 | 24 | 0 | 0 | 2781 | 496 | 864 | 0.0 | 0.0028 | 0 | 0 | 537173 |
| High | auth-public | logged-in-admin | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 0 | 3054 | 1036 | 1036 | 0.0 | 0.0020 | 0 | 0 | 525772 |
| Medium | auth-public | logged-out | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 0 | 2867 | 688 | 688 | 0.0 | 0.0000 | 0 | 0 | 525362 |
| Medium | auth-public | logged-out | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 0 | 2825 | 648 | 648 | 0.0 | 0.0000 | 0 | 0 | 525374 |
| Medium | auth-public | logged-out | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 0 | 2866 | 580 | 908 | 0.0 | 0.0000 | 0 | 0 | 525548 |
| Medium | auth-public | logged-in-admin | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 0 | 2829 | 744 | 1140 | 0.0 | 0.0020 | 0 | 0 | 525749 |
| Medium | auth-public | logged-in-admin | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 0 | 2853 | 836 | 1228 | 0.0 | 0.0020 | 0 | 0 | 525904 |
| Medium | auth-public | logged-in-reader | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 0 | 2747 | 548 | 848 | 0.0 | 0.0018 | 0 | 0 | 525765 |
| Medium | auth-public | logged-in-reader | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 0 | 2803 | 744 | 1104 | 0.0 | 0.0018 | 0 | 0 | 525727 |
| Medium | auth-public | logged-in-reader | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 0 | 2714 | 632 | 952 | 0.0 | 0.0018 | 0 | 0 | 525878 |
| Medium | auth-public | logged-out | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 0 | 2788 | 672 | 988 | 0.0 | 0.0000 | 0 | 0 | 525363 |
| Medium | auth-public | logged-out | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 0 | 2751 | 524 | 524 | 0.0 | 0.0000 | 0 | 0 | 525336 |
| Medium | auth-public | logged-out | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 0 | 2782 | 592 | 592 | 0.0 | 0.0000 | 0 | 0 | 525528 |
| Medium | auth-public | logged-in-admin | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 0 | 2686 | 544 | 860 | 0.0 | 0.0032 | 0 | 0 | 525630 |
| Medium | auth-public | logged-in-admin | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 0 | 2688 | 432 | 768 | 0.0 | 0.0032 | 0 | 0 | 525741 |
| Medium | auth-public | logged-in-admin | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 0 | 2760 | 548 | 864 | 0.0 | 0.0032 | 0 | 0 | 525790 |
| Medium | auth-public | logged-in-reader | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 0 | 2698 | 464 | 780 | 0.0 | 0.0028 | 0 | 0 | 525610 |
| Medium | auth-public | logged-in-reader | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 0 | 2670 | 556 | 880 | 0.0 | 0.0028 | 0 | 0 | 525749 |
| Medium | auth-public | logged-in-reader | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 0 | 2698 | 520 | 844 | 0.0 | 0.0028 | 0 | 0 | 525834 |
| Medium | auth-public | logged-out | desktop | /vi/login | 24 | 0 | 22 | 0 | 0 | 2832 | 716 | 716 | 0.0 | 0.0000 | 0 | 0 | 511932 |
| Medium | auth-public | logged-out | desktop | /vi/register | 24 | 0 | 22 | 0 | 0 | 2862 | 548 | 548 | 0.0 | 0.0000 | 0 | 0 | 512278 |
| Medium | auth-public | logged-out | desktop | /vi/forgot-password | 24 | 0 | 22 | 0 | 0 | 2843 | 716 | 716 | 0.0 | 0.0000 | 0 | 0 | 511408 |
| Medium | auth-public | logged-out | desktop | /vi/reset-password | 24 | 0 | 22 | 0 | 0 | 2903 | 792 | 792 | 0.0 | 0.0000 | 0 | 0 | 511508 |
| Medium | auth-public | logged-out | desktop | /vi/verify-email | 24 | 0 | 22 | 0 | 0 | 2909 | 760 | 760 | 0.0 | 0.0000 | 0 | 0 | 511636 |
| Medium | auth-public | logged-out | mobile | /vi/login | 24 | 0 | 22 | 0 | 0 | 2793 | 640 | 640 | 0.0 | 0.0000 | 0 | 0 | 511831 |
| Medium | auth-public | logged-out | mobile | /vi/register | 24 | 0 | 22 | 0 | 0 | 2938 | 576 | 576 | 0.0 | 0.0000 | 0 | 0 | 512257 |
| Medium | auth-public | logged-out | mobile | /vi/forgot-password | 24 | 0 | 22 | 0 | 0 | 2789 | 672 | 672 | 0.0 | 0.0000 | 0 | 0 | 511318 |
| Medium | auth-public | logged-out | mobile | /vi/reset-password | 24 | 0 | 22 | 0 | 0 | 2705 | 616 | 616 | 0.0 | 0.0000 | 0 | 0 | 511579 |
| Medium | auth-public | logged-out | mobile | /vi/verify-email | 24 | 0 | 22 | 0 | 0 | 2826 | 780 | 780 | 0.0 | 0.0000 | 0 | 0 | 511653 |

## Major Issues Found

- Critical: 2 page(s) có request count >35, pending request, failed request, hoặc issue nghiêm trọng.
- High: 121 page(s) vượt ngưỡng >25 requests hoặc có request >800ms.
- Medium: 27 page(s) có request trong dải 400-800ms.
- Duplicate: chưa phát hiện duplicate request business đáng kể.
- Pending: 2 page(s) có pending request không phải websocket/eventsource.

### Slow Requests

| Severity | Feature | Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| Critical | auth-public | logged-out | desktop | /vi | GET | 200 | 1339 | 582 | html | https://www.tarotnow.xyz/vi |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1227 | 484 | same-site-media | https://img.tarotnow.xyz/light-god-50/08_The_Chariot_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1211 | 242 | same-site-media | https://img.tarotnow.xyz/light-god-50/07_The_Lovers_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers | GET | 200 | 1095 | 514 | html | https://www.tarotnow.xyz/vi/readers |
| Critical | auth-public | logged-out | mobile | /vi | GET | 200 | 1082 | 575 | html | https://www.tarotnow.xyz/vi |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1014 | 152 | same-site-media | https://img.tarotnow.xyz/light-god-50/06_The_Hierophant_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet | GET | 200 | 1009 | 355 | html | https://www.tarotnow.xyz/vi/wallet |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/notifications | GET | 200 | 991 | 401 | html | https://www.tarotnow.xyz/vi/notifications |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | GET | 200 | 978 | 453 | html | https://www.tarotnow.xyz/vi/profile |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | GET | 200 | 931 | 408 | html | https://www.tarotnow.xyz/vi/profile/reader |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 924 | 172 | same-site-media | https://img.tarotnow.xyz/light-god-50/05_The_Emperor_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | GET | 200 | 922 | 875 | same-site-media | https://media.tarotnow.xyz/icon/power_booster_50_20260416_165453.avif |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | GET | 200 | 922 | 338 | html | https://www.tarotnow.xyz/vi/gamification |
| Critical | auth-public | logged-in-admin | desktop | /vi | GET | 200 | 921 | 428 | html | https://www.tarotnow.xyz/vi |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile | GET | 200 | 910 | 383 | html | https://www.tarotnow.xyz/vi/profile |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/8bf4b9ae-fcba-41af-8462-4163a214cb55 | GET | 200 | 907 | 448 | html | https://www.tarotnow.xyz/vi/reading/session/8bf4b9ae-fcba-41af-8462-4163a214cb55 |
| Critical | reading | logged-in-admin | desktop | /vi/reading | GET | 200 | 900 | 382 | html | https://www.tarotnow.xyz/vi/reading |
| Critical | reader-chat | logged-in-admin | desktop | /vi/chat | GET | 200 | 893 | 410 | html | https://www.tarotnow.xyz/vi/chat |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | GET | 200 | 886 | 395 | html | https://www.tarotnow.xyz/vi/inventory |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | GET | 200 | 884 | 846 | same-site-media | https://media.tarotnow.xyz/icon/defense_booster_50_20260416_165452.avif |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | GET | 200 | 883 | 396 | html | https://www.tarotnow.xyz/vi/wallet |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | GET | 200 | 881 | 433 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| Critical | auth-public | logged-in-admin | desktop | /vi/legal/tos | GET | 200 | 880 | 463 | html | https://www.tarotnow.xyz/vi/legal/tos |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet | GET | 200 | 880 | 416 | html | https://www.tarotnow.xyz/vi/wallet |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 877 | 343 | html | https://www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 868 | 315 | html | https://www.tarotnow.xyz/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha/history | GET | 200 | 856 | 355 | html | https://www.tarotnow.xyz/vi/gacha/history |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha | GET | 200 | 856 | 299 | html | https://www.tarotnow.xyz/vi/gacha |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit/history | GET | 200 | 855 | 305 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | GET | 200 | 849 | 749 | same-site-media | https://media.tarotnow.xyz/icon/exp_booster_50_20260416_165452.avif |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | GET | 200 | 840 | 321 | html | https://www.tarotnow.xyz/vi/profile |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/inventory | GET | 200 | 838 | 340 | html | https://www.tarotnow.xyz/vi/inventory |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha | GET | 200 | 830 | 367 | html | https://www.tarotnow.xyz/vi/gacha |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers | GET | 200 | 829 | 354 | html | https://www.tarotnow.xyz/vi/readers |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit/history | GET | 200 | 828 | 328 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/inventory | GET | 200 | 828 | 346 | html | https://www.tarotnow.xyz/vi/inventory |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/inventory | GET | 200 | 823 | 304 | html | https://www.tarotnow.xyz/vi/inventory |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/gamification | GET | 200 | 823 | 333 | html | https://www.tarotnow.xyz/vi/gamification |
| Critical | community-leaderboard-quest | logged-in-reader | desktop | /vi/leaderboard | GET | 200 | 822 | 317 | html | https://www.tarotnow.xyz/vi/leaderboard |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/f954a1be-52f2-4c60-be02-9a3cd82a91b1 | GET | 200 | 819 | 314 | html | https://www.tarotnow.xyz/vi/reading/session/f954a1be-52f2-4c60-be02-9a3cd82a91b1 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | GET | 200 | 818 | 308 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha | GET | 200 | 817 | 321 | html | https://www.tarotnow.xyz/vi/gacha |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit | GET | 200 | 816 | 408 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 815 | 207 | same-site-media | https://img.tarotnow.xyz/light-god-50/08_The_Chariot_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/withdraw | GET | 200 | 814 | 310 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 811 | 327 | html | https://www.tarotnow.xyz/vi/collection |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/leaderboard | GET | 200 | 811 | 805 | static | https://www.tarotnow.xyz/_next/static/chunks/1828ifay.e5gf.js |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers | GET | 200 | 811 | 316 | html | https://www.tarotnow.xyz/vi/readers |
| Critical | reading | logged-in-reader | mobile | /vi/reading | GET | 200 | 811 | 319 | html | https://www.tarotnow.xyz/vi/reading |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile | GET | 200 | 811 | 320 | html | https://www.tarotnow.xyz/vi/profile |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit/history | GET | 200 | 809 | 382 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet | GET | 200 | 809 | 304 | html | https://www.tarotnow.xyz/vi/wallet |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 807 | 121 | same-site-media | https://img.tarotnow.xyz/light-god-50/04_The_Empress_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | reading | logged-in-admin | mobile | /vi/reading | GET | 200 | 806 | 314 | html | https://www.tarotnow.xyz/vi/reading |
| Critical | auth-public | logged-in-admin | desktop | /vi | GET | 200 | 804 | 650 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha/history | GET | 200 | 803 | 372 | html | https://www.tarotnow.xyz/vi/gacha/history |
| Medium | reading | logged-in-reader | desktop | /vi/reading/session/1c308e30-0171-4814-8f83-bff4919171ee | GET | 200 | 799 | 295 | html | https://www.tarotnow.xyz/vi/reading/session/1c308e30-0171-4814-8f83-bff4919171ee |
| Medium | reader-chat | logged-in-reader | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 798 | 312 | html | https://www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| Medium | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet | GET | 200 | 797 | 353 | html | https://www.tarotnow.xyz/vi/wallet |
| Medium | reader-chat | logged-in-reader | desktop | /vi/readers | GET | 200 | 796 | 318 | html | https://www.tarotnow.xyz/vi/readers |
| Medium | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | GET | 200 | 795 | 313 | html | https://www.tarotnow.xyz/vi/wallet |
| Medium | auth-public | logged-in-admin | desktop | /vi | GET | 200 | 794 | 221 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| Medium | reading | logged-in-admin | desktop | /vi/reading/session/4d43e530-faa8-45de-8fd0-aa7c2575c4d5 | GET | 200 | 794 | 356 | html | https://www.tarotnow.xyz/vi/reading/session/4d43e530-faa8-45de-8fd0-aa7c2575c4d5 |
| Medium | reader-chat | logged-in-reader | desktop | /vi/chat | GET | 200 | 793 | 311 | html | https://www.tarotnow.xyz/vi/chat |
| Medium | reading | logged-in-admin | mobile | /vi/reading/session/b307c06a-d79a-48d4-a939-73621cc5752f | GET | 200 | 793 | 318 | html | https://www.tarotnow.xyz/vi/reading/session/b307c06a-d79a-48d4-a939-73621cc5752f |
| Medium | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile | GET | 200 | 788 | 373 | html | https://www.tarotnow.xyz/vi/profile |
| Medium | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/mfa | GET | 200 | 788 | 323 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| Medium | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 787 | 210 | same-site-media | https://img.tarotnow.xyz/light-god-50/07_The_Lovers_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| Medium | community-leaderboard-quest | logged-in-reader | desktop | /vi/gamification | GET | 200 | 786 | 313 | html | https://www.tarotnow.xyz/vi/gamification |
| Medium | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | GET | 200 | 785 | 314 | html | https://www.tarotnow.xyz/vi/gamification |
| Medium | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha/history | GET | 200 | 784 | 301 | html | https://www.tarotnow.xyz/vi/gacha/history |
| Medium | reader-chat | logged-in-admin | desktop | /vi/reader/apply | GET | 200 | 783 | 403 | html | https://www.tarotnow.xyz/vi/reader/apply |
| Medium | auth-public | logged-in-admin | desktop | /vi | GET | 200 | 781 | 244 | static | https://www.tarotnow.xyz/_next/static/chunks/11nim~osfalfg.js |
| Medium | auth-public | logged-in-admin | desktop | /vi/legal/ai-disclaimer | GET | 200 | 780 | 406 | html | https://www.tarotnow.xyz/vi/legal/ai-disclaimer |
| Medium | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | GET | 200 | 780 | 311 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| Medium | auth-public | logged-in-admin | desktop | /vi | GET | 200 | 779 | 655 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Medium | profile-wallet-notifications | logged-in-reader | desktop | /vi/notifications | GET | 200 | 779 | 322 | html | https://www.tarotnow.xyz/vi/notifications |
| Medium | reading | logged-in-reader | desktop | /vi/reading | GET | 200 | 778 | 297 | html | https://www.tarotnow.xyz/vi/reading |
| Medium | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha/history | GET | 200 | 778 | 307 | html | https://www.tarotnow.xyz/vi/gacha/history |
| Medium | auth-public | logged-out | desktop | /vi/register | GET | 200 | 777 | 355 | html | https://www.tarotnow.xyz/vi/register |

### Duplicate API / Request Candidates

| Severity | Feature | Scenario | Viewport | Route | Count | Request Key |
| --- | --- | --- | --- | --- | ---: | --- |
| - | - | - | - | - | - | - |

### Pending Requests

| Severity | Feature | Scenario | Viewport | Route | URL |
| --- | --- | --- | --- | --- | --- |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |

### Top Transfer Contributors

| Feature | Scenario | Viewport | Route | Category | Transfer bytes | Duration (ms) | Cache-Control | URL |
| --- | --- | --- | --- | --- | ---: | ---: | --- | --- |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | same-site-media | 325866 | 201 | max-age=14400 | img.tarotnow.xyz/light-god-50/04_The_Empress_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | same-site-media | 325854 | 807 | max-age=14400 | img.tarotnow.xyz/light-god-50/04_The_Empress_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | same-site-media | 325838 | 435 | max-age=14400 | img.tarotnow.xyz/light-god-50/04_The_Empress_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | same-site-media | 325831 | 201 | max-age=14400 | img.tarotnow.xyz/light-god-50/04_The_Empress_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | same-site-media | 259849 | 485 | max-age=14400 | img.tarotnow.xyz/light-god-50/01_The_Fool_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | same-site-media | 259844 | 213 | max-age=14400 | img.tarotnow.xyz/light-god-50/01_The_Fool_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | same-site-media | 259839 | 622 | max-age=14400 | img.tarotnow.xyz/light-god-50/01_The_Fool_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | same-site-media | 259822 | 211 | max-age=14400 | img.tarotnow.xyz/light-god-50/01_The_Fool_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | same-site-media | 259778 | 212 | max-age=14400 | img.tarotnow.xyz/light-god-50/01_The_Fool_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | same-site-media | 231154 | 616 | max-age=14400 | img.tarotnow.xyz/light-god-50/06_The_Hierophant_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | same-site-media | 231147 | 1014 | max-age=14400 | img.tarotnow.xyz/light-god-50/06_The_Hierophant_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | same-site-media | 231135 | 467 | max-age=14400 | img.tarotnow.xyz/light-god-50/06_The_Hierophant_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | same-site-media | 231119 | 572 | max-age=14400 | img.tarotnow.xyz/light-god-50/06_The_Hierophant_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | same-site-media | 226182 | 284 | max-age=14400 | img.tarotnow.xyz/light-god-50/02_The_Magician_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | same-site-media | 226144 | 217 | max-age=14400 | img.tarotnow.xyz/light-god-50/02_The_Magician_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | same-site-media | 226143 | 134 | max-age=14400 | img.tarotnow.xyz/light-god-50/02_The_Magician_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | same-site-media | 226139 | 175 | max-age=14400 | img.tarotnow.xyz/light-god-50/02_The_Magician_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | same-site-media | 226129 | 349 | max-age=14400 | img.tarotnow.xyz/light-god-50/02_The_Magician_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | same-site-media | 226129 | 409 | max-age=14400 | img.tarotnow.xyz/light-god-50/02_The_Magician_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | same-site-media | 226123 | 322 | max-age=14400 | img.tarotnow.xyz/light-god-50/02_The_Magician_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | same-site-media | 226101 | 357 | max-age=14400 | img.tarotnow.xyz/light-god-50/02_The_Magician_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | same-site-media | 211523 | 495 | max-age=14400 | img.tarotnow.xyz/light-god-50/07_The_Lovers_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | same-site-media | 211460 | 1211 | max-age=14400 | img.tarotnow.xyz/light-god-50/07_The_Lovers_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | same-site-media | 211458 | 787 | max-age=14400 | img.tarotnow.xyz/light-god-50/07_The_Lovers_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | same-site-media | 211451 | 554 | max-age=14400 | img.tarotnow.xyz/light-god-50/07_The_Lovers_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | same-site-media | 186850 | 924 | max-age=14400 | img.tarotnow.xyz/light-god-50/05_The_Emperor_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | same-site-media | 186841 | 531 | max-age=14400 | img.tarotnow.xyz/light-god-50/05_The_Emperor_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | same-site-media | 186814 | 180 | max-age=14400 | img.tarotnow.xyz/light-god-50/05_The_Emperor_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | same-site-media | 186797 | 160 | max-age=14400 | img.tarotnow.xyz/light-god-50/05_The_Emperor_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | same-site-media | 173322 | 155 | max-age=14400 | img.tarotnow.xyz/light-god-50/03_The_High%20Priestess%20_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | same-site-media | 173317 | 497 | max-age=14400 | img.tarotnow.xyz/light-god-50/03_The_High%20Priestess%20_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | same-site-media | 173294 | 124 | max-age=14400 | img.tarotnow.xyz/light-god-50/03_The_High%20Priestess%20_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | same-site-media | 173294 | 681 | max-age=14400 | img.tarotnow.xyz/light-god-50/03_The_High%20Priestess%20_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | same-site-media | 173290 | 322 | max-age=14400 | img.tarotnow.xyz/light-god-50/03_The_High%20Priestess%20_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | same-site-media | 173284 | 263 | max-age=14400 | img.tarotnow.xyz/light-god-50/03_The_High%20Priestess%20_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | same-site-media | 173280 | 217 | max-age=14400 | img.tarotnow.xyz/light-god-50/03_The_High%20Priestess%20_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | same-site-media | 173261 | 123 | max-age=14400 | img.tarotnow.xyz/light-god-50/03_The_High%20Priestess%20_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | same-site-media | 155935 | 815 | max-age=14400 | img.tarotnow.xyz/light-god-50/08_The_Chariot_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | same-site-media | 155909 | 1227 | max-age=14400 | img.tarotnow.xyz/light-god-50/08_The_Chariot_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/inventory | same-site-media | 95621 | 257 | max-age=14400 | media.tarotnow.xyz/icon/rare_title_lucky_star_50_20260416_165453.avif |
| inventory-gacha-collection | logged-in-reader | mobile | /vi/inventory | same-site-media | 95529 | 239 | max-age=14400 | media.tarotnow.xyz/icon/rare_title_lucky_star_50_20260416_165453.avif |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | same-site-media | 95526 | 754 | max-age=14400 | media.tarotnow.xyz/icon/rare_title_lucky_star_50_20260416_165453.avif |
| inventory-gacha-collection | logged-in-reader | desktop | /vi/inventory | same-site-media | 95493 | 283 | max-age=14400 | media.tarotnow.xyz/icon/rare_title_lucky_star_50_20260416_165453.avif |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha | same-site-media | 95481 | 191 | max-age=14400 | media.tarotnow.xyz/icon/rare_title_lucky_star_50_20260416_165453.avif |
| inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha | same-site-media | 95475 | 152 | max-age=14400 | media.tarotnow.xyz/icon/rare_title_lucky_star_50_20260416_165453.avif |
| inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha | same-site-media | 95475 | 167 | max-age=14400 | media.tarotnow.xyz/icon/rare_title_lucky_star_50_20260416_165453.avif |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha | same-site-media | 95463 | 174 | max-age=14400 | media.tarotnow.xyz/icon/rare_title_lucky_star_50_20260416_165453.avif |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha | static | 89058 | 120 | public, max-age=31536000, immutable | www.tarotnow.xyz/_next/static/chunks/125yw.nzq306p.js |
| inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha | static | 89057 | 119 | public, max-age=31536000, immutable | www.tarotnow.xyz/_next/static/chunks/125yw.nzq306p.js |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha/history | static | 89055 | 124 | public, max-age=31536000, immutable | www.tarotnow.xyz/_next/static/chunks/125yw.nzq306p.js |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha | static | 89054 | 121 | public, max-age=31536000, immutable | www.tarotnow.xyz/_next/static/chunks/125yw.nzq306p.js |
| inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha/history | static | 89052 | 125 | public, max-age=31536000, immutable | www.tarotnow.xyz/_next/static/chunks/125yw.nzq306p.js |
| inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha | static | 89052 | 100 | public, max-age=31536000, immutable | www.tarotnow.xyz/_next/static/chunks/125yw.nzq306p.js |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha/history | static | 89050 | 225 | public, max-age=31536000, immutable | www.tarotnow.xyz/_next/static/chunks/125yw.nzq306p.js |
| inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha/history | static | 89050 | 129 | public, max-age=31536000, immutable | www.tarotnow.xyz/_next/static/chunks/125yw.nzq306p.js |
| reader-chat | logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | static | 81463 | 410 | public, max-age=31536000, immutable | www.tarotnow.xyz/_next/static/chunks/11nim~osfalfg.js |
| reader-chat | logged-in-reader | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | static | 81456 | 457 | public, max-age=31536000, immutable | www.tarotnow.xyz/_next/static/chunks/11nim~osfalfg.js |
| reading | logged-in-reader | mobile | /vi/reading/session/b2c27c49-ed23-482e-a97a-58bd1e3f893b | static | 81454 | 410 | public, max-age=31536000, immutable | www.tarotnow.xyz/_next/static/chunks/11nim~osfalfg.js |
| auth-public | logged-out | desktop | /vi/forgot-password | static | 81449 | 441 | public, max-age=31536000, immutable | www.tarotnow.xyz/_next/static/chunks/11nim~osfalfg.js |
| reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | static | 81446 | 251 | public, max-age=31536000, immutable | www.tarotnow.xyz/_next/static/chunks/11nim~osfalfg.js |
| auth-public | logged-out | desktop | /vi/legal/tos | static | 81445 | 465 | public, max-age=31536000, immutable | www.tarotnow.xyz/_next/static/chunks/11nim~osfalfg.js |
| auth-public | logged-in-admin | desktop | /vi/legal/tos | static | 81445 | 519 | public, max-age=31536000, immutable | www.tarotnow.xyz/_next/static/chunks/11nim~osfalfg.js |
| reader-chat | logged-in-admin | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | static | 81445 | 293 | public, max-age=31536000, immutable | www.tarotnow.xyz/_next/static/chunks/11nim~osfalfg.js |
| reader-chat | logged-in-reader | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | static | 81444 | 350 | public, max-age=31536000, immutable | www.tarotnow.xyz/_next/static/chunks/11nim~osfalfg.js |
| profile-wallet-notifications | logged-in-admin | desktop | /vi/profile | static | 81443 | 292 | public, max-age=31536000, immutable | www.tarotnow.xyz/_next/static/chunks/11nim~osfalfg.js |
| profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | static | 81443 | 498 | public, max-age=31536000, immutable | www.tarotnow.xyz/_next/static/chunks/11nim~osfalfg.js |
| reader-chat | logged-in-admin | desktop | /vi/reader/apply | static | 81442 | 399 | public, max-age=31536000, immutable | www.tarotnow.xyz/_next/static/chunks/11nim~osfalfg.js |
| reader-chat | logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | static | 81442 | 361 | public, max-age=31536000, immutable | www.tarotnow.xyz/_next/static/chunks/11nim~osfalfg.js |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | static | 81442 | 367 | public, max-age=31536000, immutable | www.tarotnow.xyz/_next/static/chunks/11nim~osfalfg.js |
| profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit | static | 81442 | 305 | public, max-age=31536000, immutable | www.tarotnow.xyz/_next/static/chunks/11nim~osfalfg.js |
| auth-public | logged-out | desktop | /vi/legal/privacy | static | 81441 | 426 | public, max-age=31536000, immutable | www.tarotnow.xyz/_next/static/chunks/11nim~osfalfg.js |
| auth-public | logged-in-admin | desktop | /vi/legal/privacy | static | 81441 | 346 | public, max-age=31536000, immutable | www.tarotnow.xyz/_next/static/chunks/11nim~osfalfg.js |
| auth-public | logged-in-admin | desktop | /vi/legal/ai-disclaimer | static | 81441 | 343 | public, max-age=31536000, immutable | www.tarotnow.xyz/_next/static/chunks/11nim~osfalfg.js |
| auth-public | logged-in-reader | desktop | /vi | static | 81441 | 338 | public, max-age=31536000, immutable | www.tarotnow.xyz/_next/static/chunks/11nim~osfalfg.js |
| inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha | static | 81441 | 506 | public, max-age=31536000, immutable | www.tarotnow.xyz/_next/static/chunks/11nim~osfalfg.js |
| reading | logged-in-admin | mobile | /vi/reading | static | 81441 | 425 | public, max-age=31536000, immutable | www.tarotnow.xyz/_next/static/chunks/11nim~osfalfg.js |
| profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/reader | static | 81441 | 367 | public, max-age=31536000, immutable | www.tarotnow.xyz/_next/static/chunks/11nim~osfalfg.js |
| profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet | static | 81441 | 407 | public, max-age=31536000, immutable | www.tarotnow.xyz/_next/static/chunks/11nim~osfalfg.js |
| auth-public | logged-out | desktop | /vi/register | static | 81440 | 338 | public, max-age=31536000, immutable | www.tarotnow.xyz/_next/static/chunks/11nim~osfalfg.js |
| reader-chat | logged-in-admin | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | static | 81440 | 302 | public, max-age=31536000, immutable | www.tarotnow.xyz/_next/static/chunks/11nim~osfalfg.js |

### Cacheability Issues

| Feature | Scenario | Viewport | Route | Category | Issue | Transfer bytes | Cache-Control | URL |
| --- | --- | --- | --- | --- | --- | ---: | --- | --- |
| - | - | - | - | - | - | - | - | - |

### Waterfall Sample

| Feature | Scenario | Viewport | Route | Start (ms) | End (ms) | Duration (ms) | Type | Category | URL |
| --- | --- | --- | --- | ---: | ---: | ---: | --- | --- | --- |
| auth-public | logged-out | desktop | /vi/forgot-password | 3 | 757 | 754 | document | html | www.tarotnow.xyz/vi/forgot-password |
| auth-public | logged-out | desktop | /vi/reset-password | 3 | 776 | 773 | document | html | www.tarotnow.xyz/vi/reset-password |
| auth-public | logged-out | desktop | /vi/verify-email | 3 | 735 | 732 | document | html | www.tarotnow.xyz/vi/verify-email |
| auth-public | logged-out | desktop | /vi/legal/tos | 3 | 682 | 679 | document | html | www.tarotnow.xyz/vi/legal/tos |
| auth-public | logged-out | desktop | /vi/legal/ai-disclaimer | 3 | 720 | 717 | document | html | www.tarotnow.xyz/vi/legal/ai-disclaimer |
| auth-public | logged-in-admin | mobile | /vi | 3 | 711 | 708 | document | html | www.tarotnow.xyz/vi |
| reader-chat | logged-in-admin | mobile | /vi/chat | 3 | 755 | 752 | document | html | www.tarotnow.xyz/vi/chat |
| auth-public | logged-out | desktop | /vi/register | 4 | 781 | 777 | document | html | www.tarotnow.xyz/vi/register |
| auth-public | logged-out | desktop | /vi/legal/privacy | 4 | 672 | 668 | document | html | www.tarotnow.xyz/vi/legal/privacy |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha/history | 4 | 860 | 856 | document | html | www.tarotnow.xyz/vi/gacha/history |
| profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | 4 | 885 | 881 | document | html | www.tarotnow.xyz/vi/profile/mfa |
| reader-chat | logged-in-admin | desktop | /vi/readers | 4 | 1099 | 1095 | document | html | www.tarotnow.xyz/vi/readers |
| community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | 4 | 926 | 922 | document | html | www.tarotnow.xyz/vi/gamification |
| profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet | 4 | 1013 | 1009 | document | html | www.tarotnow.xyz/vi/wallet |
| profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit/history | 4 | 813 | 809 | document | html | www.tarotnow.xyz/vi/wallet/deposit/history |
| inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | 4 | 745 | 741 | document | html | www.tarotnow.xyz/vi/collection |
| profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/withdraw | 4 | 818 | 814 | document | html | www.tarotnow.xyz/vi/wallet/withdraw |
| reader-chat | logged-in-reader | desktop | /vi/reader/apply | 4 | 650 | 646 | document | html | www.tarotnow.xyz/vi/reader/apply |
| reading | logged-in-reader | desktop | /vi/reading/history | 4 | 738 | 734 | document | html | www.tarotnow.xyz/vi/reading/history |
| reader-chat | logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 4 | 772 | 768 | document | html | www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| reader-chat | logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 4 | 707 | 703 | document | html | www.tarotnow.xyz/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| reader-chat | logged-in-reader | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 4 | 750 | 746 | document | html | www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |
| auth-public | logged-out | mobile | /vi | 4 | 1086 | 1082 | document | html | www.tarotnow.xyz/vi |
| auth-public | logged-out | mobile | /vi/login | 4 | 596 | 592 | document | html | www.tarotnow.xyz/vi/login |
| auth-public | logged-out | mobile | /vi/register | 4 | 734 | 730 | document | html | www.tarotnow.xyz/vi/register |
| auth-public | logged-out | mobile | /vi/reset-password | 4 | 611 | 607 | document | html | www.tarotnow.xyz/vi/reset-password |
| auth-public | logged-out | mobile | /vi/verify-email | 4 | 728 | 724 | document | html | www.tarotnow.xyz/vi/verify-email |
| auth-public | logged-out | mobile | /vi/legal/tos | 4 | 730 | 726 | document | html | www.tarotnow.xyz/vi/legal/tos |
| auth-public | logged-out | mobile | /vi/legal/privacy | 4 | 655 | 651 | document | html | www.tarotnow.xyz/vi/legal/privacy |
| auth-public | logged-out | mobile | /vi/legal/ai-disclaimer | 4 | 714 | 710 | document | html | www.tarotnow.xyz/vi/legal/ai-disclaimer |
| reading | logged-in-admin | mobile | /vi/reading | 4 | 810 | 806 | document | html | www.tarotnow.xyz/vi/reading |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/inventory | 4 | 842 | 838 | document | html | www.tarotnow.xyz/vi/inventory |
| profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/mfa | 4 | 792 | 788 | document | html | www.tarotnow.xyz/vi/profile/mfa |
| profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit/history | 4 | 859 | 855 | document | html | www.tarotnow.xyz/vi/wallet/deposit/history |
| auth-public | logged-in-admin | mobile | /vi/legal/privacy | 4 | 627 | 623 | document | html | www.tarotnow.xyz/vi/legal/privacy |
| reading | logged-in-admin | mobile | /vi/reading/session/c29d9601-a006-4ed4-8d19-76af37aa2a9f | 4 | 695 | 691 | document | html | www.tarotnow.xyz/vi/reading/session/c29d9601-a006-4ed4-8d19-76af37aa2a9f |
| reader-chat | logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 4 | 730 | 726 | document | html | www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| reading | logged-in-reader | mobile | /vi/reading/history | 4 | 660 | 656 | document | html | www.tarotnow.xyz/vi/reading/history |
| auth-public | logged-out | desktop | /vi/login | 5 | 612 | 607 | document | html | www.tarotnow.xyz/vi/login |
| profile-wallet-notifications | logged-in-admin | desktop | /vi/profile | 5 | 915 | 910 | document | html | www.tarotnow.xyz/vi/profile |
| profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | 5 | 733 | 728 | document | html | www.tarotnow.xyz/vi/profile/reader |
| reader-chat | logged-in-admin | desktop | /vi/chat | 5 | 898 | 893 | document | html | www.tarotnow.xyz/vi/chat |
| community-leaderboard-quest | logged-in-admin | desktop | /vi/community | 5 | 662 | 657 | document | html | www.tarotnow.xyz/vi/community |
| profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit | 5 | 821 | 816 | document | html | www.tarotnow.xyz/vi/wallet/deposit |
| profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | 5 | 683 | 678 | document | html | www.tarotnow.xyz/vi/wallet/withdraw |
| profile-wallet-notifications | logged-in-admin | desktop | /vi/notifications | 5 | 996 | 991 | document | html | www.tarotnow.xyz/vi/notifications |
| reading | logged-in-admin | desktop | /vi/reading/history | 5 | 686 | 681 | document | html | www.tarotnow.xyz/vi/reading/history |
| auth-public | logged-in-admin | desktop | /vi/legal/privacy | 5 | 699 | 694 | document | html | www.tarotnow.xyz/vi/legal/privacy |
| reading | logged-in-admin | desktop | /vi/reading/session/8bf4b9ae-fcba-41af-8462-4163a214cb55 | 5 | 912 | 907 | document | html | www.tarotnow.xyz/vi/reading/session/8bf4b9ae-fcba-41af-8462-4163a214cb55 |
| reading | logged-in-admin | desktop | /vi/reading/session/23fb1aab-7675-4486-885a-f69d5050984e | 5 | 748 | 743 | document | html | www.tarotnow.xyz/vi/reading/session/23fb1aab-7675-4486-885a-f69d5050984e |
| reader-chat | logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 5 | 723 | 718 | document | html | www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| reader-chat | logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 5 | 723 | 718 | document | html | www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| reader-chat | logged-in-admin | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 5 | 732 | 727 | document | html | www.tarotnow.xyz/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| reader-chat | logged-in-admin | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 5 | 778 | 773 | document | html | www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |
| reader-chat | logged-in-admin | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 5 | 724 | 719 | document | html | www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| reader-chat | logged-in-reader | desktop | /vi/readers | 5 | 801 | 796 | document | html | www.tarotnow.xyz/vi/readers |
| community-leaderboard-quest | logged-in-reader | desktop | /vi/gamification | 5 | 791 | 786 | document | html | www.tarotnow.xyz/vi/gamification |
| profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit/history | 5 | 833 | 828 | document | html | www.tarotnow.xyz/vi/wallet/deposit/history |
| profile-wallet-notifications | logged-in-reader | desktop | /vi/notifications | 5 | 784 | 779 | document | html | www.tarotnow.xyz/vi/notifications |
| auth-public | logged-in-reader | desktop | /vi/legal/tos | 5 | 684 | 679 | document | html | www.tarotnow.xyz/vi/legal/tos |
| auth-public | logged-in-reader | desktop | /vi/legal/ai-disclaimer | 5 | 656 | 651 | document | html | www.tarotnow.xyz/vi/legal/ai-disclaimer |
| reading | logged-in-reader | desktop | /vi/reading/session/1c308e30-0171-4814-8f83-bff4919171ee | 5 | 804 | 799 | document | html | www.tarotnow.xyz/vi/reading/session/1c308e30-0171-4814-8f83-bff4919171ee |
| reader-chat | logged-in-reader | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 5 | 778 | 773 | document | html | www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| reader-chat | logged-in-reader | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 5 | 803 | 798 | document | html | www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| auth-public | logged-out | mobile | /vi/forgot-password | 5 | 671 | 666 | document | html | www.tarotnow.xyz/vi/forgot-password |
| profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | 5 | 936 | 931 | document | html | www.tarotnow.xyz/vi/profile/reader |
| community-leaderboard-quest | logged-in-admin | mobile | /vi/community | 5 | 764 | 759 | document | html | www.tarotnow.xyz/vi/community |
| community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 5 | 790 | 785 | document | html | www.tarotnow.xyz/vi/gamification |
| profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit | 5 | 660 | 655 | document | html | www.tarotnow.xyz/vi/wallet/deposit |
| profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 5 | 785 | 780 | document | html | www.tarotnow.xyz/vi/wallet/withdraw |
| profile-wallet-notifications | logged-in-admin | mobile | /vi/notifications | 5 | 653 | 648 | document | html | www.tarotnow.xyz/vi/notifications |
| reader-chat | logged-in-admin | mobile | /vi/reader/apply | 5 | 763 | 758 | document | html | www.tarotnow.xyz/vi/reader/apply |
| auth-public | logged-in-admin | mobile | /vi/legal/tos | 5 | 631 | 626 | document | html | www.tarotnow.xyz/vi/legal/tos |
| auth-public | logged-in-admin | mobile | /vi/legal/ai-disclaimer | 5 | 706 | 701 | document | html | www.tarotnow.xyz/vi/legal/ai-disclaimer |
| reading | logged-in-admin | mobile | /vi/reading/session/b307c06a-d79a-48d4-a939-73621cc5752f | 5 | 798 | 793 | document | html | www.tarotnow.xyz/vi/reading/session/b307c06a-d79a-48d4-a939-73621cc5752f |
| reader-chat | logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 5 | 632 | 627 | document | html | www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| inventory-gacha-collection | logged-in-reader | mobile | /vi/inventory | 5 | 833 | 828 | document | html | www.tarotnow.xyz/vi/inventory |
| inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha/history | 5 | 808 | 803 | document | html | www.tarotnow.xyz/vi/gacha/history |
| profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/mfa | 5 | 640 | 635 | document | html | www.tarotnow.xyz/vi/profile/mfa |
| profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/reader | 5 | 765 | 760 | document | html | www.tarotnow.xyz/vi/profile/reader |
| community-leaderboard-quest | logged-in-reader | mobile | /vi/community | 5 | 742 | 737 | document | html | www.tarotnow.xyz/vi/community |
| community-leaderboard-quest | logged-in-reader | mobile | /vi/gamification | 5 | 828 | 823 | document | html | www.tarotnow.xyz/vi/gamification |
| profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/withdraw | 5 | 777 | 772 | document | html | www.tarotnow.xyz/vi/wallet/withdraw |
| reader-chat | logged-in-reader | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 5 | 622 | 617 | document | html | www.tarotnow.xyz/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| reader-chat | logged-in-reader | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 5 | 666 | 661 | document | html | www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | 6 | 892 | 886 | document | html | www.tarotnow.xyz/vi/inventory |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha | 6 | 836 | 830 | document | html | www.tarotnow.xyz/vi/gacha |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | 6 | 817 | 811 | document | html | www.tarotnow.xyz/vi/collection |
| community-leaderboard-quest | logged-in-admin | desktop | /vi/leaderboard | 6 | 718 | 712 | document | html | www.tarotnow.xyz/vi/leaderboard |
| auth-public | logged-in-admin | desktop | /vi/legal/tos | 6 | 886 | 880 | document | html | www.tarotnow.xyz/vi/legal/tos |
| auth-public | logged-in-admin | desktop | /vi/legal/ai-disclaimer | 6 | 786 | 780 | document | html | www.tarotnow.xyz/vi/legal/ai-disclaimer |
| reader-chat | logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | 6 | 768 | 762 | document | html | www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| auth-public | logged-in-reader | desktop | /vi | 6 | 700 | 694 | document | html | www.tarotnow.xyz/vi |
| inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha | 6 | 862 | 856 | document | html | www.tarotnow.xyz/vi/gacha |
| profile-wallet-notifications | logged-in-reader | desktop | /vi/profile | 6 | 783 | 777 | document | html | www.tarotnow.xyz/vi/profile |
| profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/mfa | 6 | 733 | 727 | document | html | www.tarotnow.xyz/vi/profile/mfa |
| community-leaderboard-quest | logged-in-reader | desktop | /vi/leaderboard | 6 | 828 | 822 | document | html | www.tarotnow.xyz/vi/leaderboard |
| community-leaderboard-quest | logged-in-reader | desktop | /vi/community | 6 | 755 | 749 | document | html | www.tarotnow.xyz/vi/community |
| profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet | 6 | 815 | 809 | document | html | www.tarotnow.xyz/vi/wallet |
| auth-public | logged-in-reader | desktop | /vi/legal/privacy | 6 | 719 | 713 | document | html | www.tarotnow.xyz/vi/legal/privacy |
| reading | logged-in-reader | desktop | /vi/reading/session/f01fe337-50d1-47c7-a39b-94da807fc70c | 6 | 719 | 713 | document | html | www.tarotnow.xyz/vi/reading/session/f01fe337-50d1-47c7-a39b-94da807fc70c |
| reading | logged-in-reader | desktop | /vi/reading/session/4100878a-d5b0-4d16-a391-b4453647f29b | 6 | 734 | 728 | document | html | www.tarotnow.xyz/vi/reading/session/4100878a-d5b0-4d16-a391-b4453647f29b |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha | 6 | 823 | 817 | document | html | www.tarotnow.xyz/vi/gacha |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha/history | 6 | 784 | 778 | document | html | www.tarotnow.xyz/vi/gacha/history |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | 6 | 759 | 753 | document | html | www.tarotnow.xyz/vi/collection |
| profile-wallet-notifications | logged-in-admin | mobile | /vi/profile | 6 | 794 | 788 | document | html | www.tarotnow.xyz/vi/profile |
| reader-chat | logged-in-admin | mobile | /vi/readers | 6 | 817 | 811 | document | html | www.tarotnow.xyz/vi/readers |
| community-leaderboard-quest | logged-in-admin | mobile | /vi/leaderboard | 6 | 757 | 751 | document | html | www.tarotnow.xyz/vi/leaderboard |
| reading | logged-in-admin | mobile | /vi/reading/history | 6 | 766 | 760 | document | html | www.tarotnow.xyz/vi/reading/history |
| reading | logged-in-admin | mobile | /vi/reading/session/f954a1be-52f2-4c60-be02-9a3cd82a91b1 | 6 | 825 | 819 | document | html | www.tarotnow.xyz/vi/reading/session/f954a1be-52f2-4c60-be02-9a3cd82a91b1 |
| reader-chat | logged-in-admin | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 6 | 662 | 656 | document | html | www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| auth-public | logged-in-reader | mobile | /vi | 6 | 714 | 708 | document | html | www.tarotnow.xyz/vi |
| reading | logged-in-reader | mobile | /vi/reading | 6 | 817 | 811 | document | html | www.tarotnow.xyz/vi/reading |
| profile-wallet-notifications | logged-in-reader | mobile | /vi/profile | 6 | 817 | 811 | document | html | www.tarotnow.xyz/vi/profile |
| reader-chat | logged-in-reader | mobile | /vi/readers | 6 | 835 | 829 | document | html | www.tarotnow.xyz/vi/readers |
| community-leaderboard-quest | logged-in-reader | mobile | /vi/leaderboard | 6 | 764 | 758 | document | html | www.tarotnow.xyz/vi/leaderboard |
| profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet | 6 | 886 | 880 | document | html | www.tarotnow.xyz/vi/wallet |
| profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit | 6 | 683 | 677 | document | html | www.tarotnow.xyz/vi/wallet/deposit |
| profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit/history | 6 | 634 | 628 | document | html | www.tarotnow.xyz/vi/wallet/deposit/history |
| profile-wallet-notifications | logged-in-reader | mobile | /vi/notifications | 6 | 773 | 767 | document | html | www.tarotnow.xyz/vi/notifications |

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
