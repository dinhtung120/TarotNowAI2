# PERFORMANCE AUDIT - TarotNow

## Executive Summary

- Generated at (UTC): 2026-05-08T13:19:50.003Z
- Benchmark generated at (UTC): 2026-05-08T13:19:39.923Z
- Base origin: https://www.tarotnow.xyz
- Locale prefix: /vi
- Benchmark mode: full-matrix
- Total scenarios: 6
- Total pages measured: 196
- Critical pages: 34
- High pages: 139
- Medium pages: 23
- Slow requests >800ms: 132
- Slow requests 400-800ms: 545
- Request thresholds: >25 suspicious, >35 severe
- Slow request thresholds: >400ms optimize, >800ms serious

## Scenario Coverage

| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Failed | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 25.3 | 3058 | 0 | 1 | yes |
| logged-in-admin | desktop | 50 | 30.7 | 3126 | 4 | 5 | yes |
| logged-in-reader | desktop | 40 | 30.4 | 3057 | 0 | 5 | yes |
| logged-out | mobile | 9 | 25.2 | 3677 | 2 | 1 | yes |
| logged-in-admin | mobile | 49 | 30.7 | 3057 | 0 | 8 | yes |
| logged-in-reader | mobile | 39 | 30.1 | 2992 | 1 | 4 | yes |

## Route Family Coverage

| Scenario | Viewport | Family | Pages | Avg requests/page | Avg nav (ms) | Pending | Failed |
| --- | --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.6 | 3002 | 0 | 0 |
| logged-in-admin | desktop | auth-public | 8 | 35.3 | 3120 | 1 | 3 |
| logged-in-admin | desktop | community-leaderboard-quest | 4 | 28.0 | 3268 | 0 | 2 |
| logged-in-admin | desktop | home | 1 | 26.0 | 2734 | 0 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | 4 | 35.3 | 4210 | 0 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | 8 | 29.5 | 2935 | 2 | 0 |
| logged-in-admin | desktop | reader-chat | 9 | 28.2 | 2829 | 1 | 0 |
| logged-in-admin | desktop | reading | 6 | 31.5 | 3295 | 0 | 0 |
| logged-in-admin | mobile | admin | 10 | 29.8 | 2975 | 0 | 0 |
| logged-in-admin | mobile | auth-public | 8 | 35.8 | 3105 | 0 | 3 |
| logged-in-admin | mobile | community-leaderboard-quest | 4 | 25.8 | 3027 | 0 | 1 |
| logged-in-admin | mobile | home | 1 | 26.0 | 2776 | 0 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | 4 | 33.0 | 3691 | 0 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | 8 | 28.9 | 2860 | 0 | 0 |
| logged-in-admin | mobile | reader-chat | 9 | 31.8 | 3122 | 0 | 4 |
| logged-in-admin | mobile | reading | 5 | 28.0 | 2912 | 0 | 0 |
| logged-in-reader | desktop | auth-public | 8 | 35.4 | 3167 | 0 | 3 |
| logged-in-reader | desktop | community-leaderboard-quest | 4 | 24.8 | 2948 | 0 | 1 |
| logged-in-reader | desktop | home | 1 | 26.0 | 2724 | 0 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | 4 | 33.0 | 3776 | 0 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | 8 | 29.6 | 2898 | 0 | 1 |
| logged-in-reader | desktop | reader-chat | 9 | 29.8 | 3060 | 0 | 0 |
| logged-in-reader | desktop | reading | 6 | 28.2 | 2765 | 0 | 0 |
| logged-in-reader | mobile | auth-public | 8 | 34.1 | 2968 | 0 | 3 |
| logged-in-reader | mobile | community-leaderboard-quest | 4 | 24.8 | 2926 | 0 | 1 |
| logged-in-reader | mobile | home | 1 | 26.0 | 2922 | 0 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | 4 | 33.0 | 3598 | 0 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | 8 | 29.6 | 2940 | 0 | 0 |
| logged-in-reader | mobile | reader-chat | 9 | 28.1 | 2804 | 1 | 0 |
| logged-in-reader | mobile | reading | 5 | 31.0 | 3032 | 0 | 0 |
| logged-out | desktop | auth-public | 8 | 24.5 | 2830 | 0 | 0 |
| logged-out | desktop | home | 1 | 32.0 | 4877 | 0 | 1 |
| logged-out | mobile | auth-public | 8 | 24.5 | 3647 | 0 | 0 |
| logged-out | mobile | home | 1 | 31.0 | 3914 | 2 | 1 |

## Detailed Metrics Table

| Severity | Feature | Scenario | Viewport | Route | Requests | API | Static | Third-party | Nav (ms) | FCP (ms) | LCP (ms) | TBT (ms) | CLS | Pending | Failed | Transfer bytes |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers | 62 | 6 | 52 | 0 | 4149 | 920 | 1280 | 0.0 | 0.0000 | 0 | 4 | 1208430 |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | 60 | 5 | 50 | 0 | 3931 | 600 | 1100 | 189.0 | 0.0033 | 0 | 1 | 1114731 |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | 60 | 5 | 50 | 0 | 3688 | 816 | 816 | 0.0 | 0.0051 | 0 | 1 | 1115664 |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | 53 | 0 | 49 | 0 | 3720 | 1136 | 1136 | 27.0 | 0.0039 | 0 | 1 | 1108401 |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | 53 | 0 | 49 | 0 | 3666 | 932 | 932 | 114.0 | 0.0039 | 1 | 1 | 1108547 |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | 50 | 0 | 46 | 0 | 3576 | 748 | 1256 | 148.0 | 0.0035 | 0 | 1 | 1038252 |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | 50 | 0 | 46 | 0 | 3253 | 588 | 1088 | 103.0 | 0.0033 | 0 | 1 | 1038327 |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | 50 | 0 | 46 | 0 | 3320 | 652 | 1164 | 129.0 | 0.0033 | 0 | 1 | 1038288 |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | 50 | 0 | 46 | 0 | 3691 | 580 | 948 | 0.0 | 0.0051 | 0 | 1 | 1038023 |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | 50 | 0 | 46 | 0 | 3304 | 556 | 908 | 0.0 | 0.0051 | 0 | 1 | 1038044 |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | 50 | 0 | 46 | 0 | 3595 | 656 | 1020 | 0.0 | 0.0055 | 0 | 1 | 1038111 |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | 50 | 0 | 46 | 0 | 3342 | 556 | 924 | 0.0 | 0.0055 | 0 | 1 | 1038362 |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | 50 | 0 | 46 | 0 | 3309 | 584 | 940 | 0.0 | 0.0055 | 0 | 1 | 1038312 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | 41 | 5 | 34 | 0 | 3637 | 796 | 1176 | 0.0 | 0.0039 | 0 | 0 | 665841 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha | 40 | 4 | 34 | 0 | 3650 | 752 | 1128 | 0.0 | 0.0039 | 0 | 0 | 746732 |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/fa9476c1-9a62-465e-9123-3720ac2ddba3 | 39 | 5 | 32 | 0 | 3200 | 780 | 3108 | 0.0 | 0.0039 | 0 | 0 | 738281 |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/2aee7a1c-4322-427e-b5c1-30972072e89b | 39 | 6 | 31 | 0 | 3574 | 608 | 3224 | 0.0 | 0.0000 | 0 | 0 | 708025 |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/community | 38 | 3 | 33 | 0 | 4018 | 684 | 2272 | 0.0 | 0.0039 | 0 | 1 | 780033 |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/inventory | 36 | 1 | 33 | 0 | 2947 | 608 | 972 | 0.0 | 0.0039 | 0 | 0 | 653605 |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha | 36 | 1 | 33 | 0 | 2998 | 612 | 996 | 0.0 | 0.0039 | 0 | 0 | 735123 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/inventory | 36 | 1 | 33 | 0 | 2944 | 604 | 936 | 0.0 | 0.0000 | 0 | 0 | 662315 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha | 36 | 1 | 33 | 0 | 3321 | 644 | 984 | 0.0 | 0.0069 | 0 | 0 | 799166 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/inventory | 36 | 1 | 33 | 0 | 2930 | 548 | 888 | 0.0 | 0.0000 | 0 | 0 | 662700 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha | 36 | 1 | 33 | 0 | 2950 | 548 | 904 | 0.0 | 0.0000 | 0 | 0 | 799471 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile | 34 | 3 | 28 | 0 | 2974 | 624 | 976 | 0.0 | 0.0726 | 0 | 1 | 640265 |
| Critical | auth-public | logged-out | desktop | /vi | 32 | 2 | 28 | 0 | 4877 | 1332 | 1332 | 223.0 | 0.0000 | 0 | 1 | 613578 |
| Critical | auth-public | logged-out | mobile | /vi | 31 | 2 | 27 | 0 | 3914 | 1064 | 1064 | 0.0 | 0.0000 | 2 | 1 | 603560 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | 28 | 0 | 26 | 0 | 2780 | 1088 | 1456 | 0.0 | 0.0486 | 2 | 0 | 630804 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers | 27 | 0 | 25 | 0 | 2794 | 940 | 1436 | 122.0 | 0.0039 | 1 | 0 | 607724 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/chat | 27 | 0 | 25 | 0 | 2850 | 1072 | 1072 | 0.0 | 0.0055 | 1 | 0 | 607448 |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/community/69db54fc297f66f734421a3c | 9 | 0 | 7 | 0 | 2519 | 392 | 392 | 0.0 | 0.0000 | 0 | 1 | 166142 |
| Critical | community-leaderboard-quest | logged-in-reader | desktop | /vi/community/69db54fc297f66f734421a3c | 9 | 0 | 7 | 0 | 2633 | 504 | 504 | 0.0 | 0.0000 | 0 | 1 | 166162 |
| Critical | community-leaderboard-quest | logged-in-admin | mobile | /vi/community/69db54fc297f66f734421a3c | 9 | 0 | 7 | 0 | 2562 | 372 | 372 | 0.0 | 0.0000 | 0 | 1 | 166150 |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/community/69db54fc297f66f734421a3c | 9 | 0 | 7 | 0 | 2608 | 344 | 344 | 0.0 | 0.0000 | 0 | 1 | 166148 |
| High | reading | logged-in-admin | desktop | /vi/reading/history | 35 | 5 | 28 | 0 | 4494 | 604 | 2636 | 0.0 | 0.0039 | 0 | 0 | 647541 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 35 | 5 | 28 | 0 | 4886 | 728 | 2924 | 0.0 | 0.0039 | 0 | 0 | 648207 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/leaderboard | 34 | 3 | 29 | 0 | 3199 | 560 | 896 | 0.0 | 0.0196 | 0 | 0 | 654982 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile | 34 | 3 | 28 | 0 | 3072 | 548 | 1096 | 0.0 | 0.0000 | 0 | 0 | 650335 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile | 33 | 3 | 28 | 0 | 3128 | 760 | 1128 | 0.0 | 0.0486 | 0 | 0 | 640671 |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/leaderboard | 33 | 3 | 28 | 0 | 3329 | 680 | 1008 | 0.0 | 0.0177 | 0 | 0 | 653027 |
| High | admin | logged-in-admin | desktop | /vi/admin/gamification | 33 | 1 | 30 | 0 | 3464 | 912 | 1224 | 0.0 | 0.0022 | 0 | 0 | 699546 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile | 33 | 3 | 28 | 0 | 3156 | 560 | 892 | 0.0 | 0.0000 | 0 | 0 | 651951 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/withdraw | 33 | 3 | 27 | 0 | 3037 | 568 | 916 | 0.0 | 0.0330 | 0 | 0 | 639401 |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | 32 | 3 | 27 | 0 | 3205 | 656 | 1020 | 0.0 | 0.0189 | 0 | 0 | 647181 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/notifications | 32 | 3 | 27 | 0 | 3076 | 628 | 1000 | 0.0 | 0.0044 | 0 | 0 | 636308 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | 32 | 3 | 27 | 0 | 3023 | 632 | 1000 | 0.0 | 0.0039 | 0 | 0 | 635909 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | 32 | 3 | 27 | 0 | 3045 | 596 | 944 | 0.0 | 0.0039 | 0 | 0 | 635692 |
| High | admin | logged-in-admin | mobile | /vi/admin/gamification | 32 | 0 | 30 | 0 | 3032 | 568 | 888 | 0.0 | 0.0000 | 0 | 0 | 698738 |
| High | reading | logged-in-reader | mobile | /vi/reading | 32 | 3 | 27 | 0 | 2944 | 572 | 896 | 0.0 | 0.0000 | 0 | 0 | 646960 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha/history | 31 | 1 | 28 | 0 | 3087 | 732 | 1248 | 0.0 | 0.0039 | 0 | 0 | 727066 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/eb1f6399-08c3-4a62-ac47-1989f2a4a680 | 31 | 0 | 29 | 0 | 3538 | 700 | 1100 | 0.0 | 0.0039 | 0 | 0 | 712872 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 31 | 2 | 27 | 0 | 2810 | 860 | 992 | 0.0 | 0.0039 | 0 | 0 | 636130 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha/history | 31 | 1 | 28 | 0 | 2880 | 588 | 1108 | 0.0 | 0.0039 | 0 | 0 | 725452 |
| High | reader-chat | logged-in-reader | desktop | /vi/reader/apply | 31 | 2 | 27 | 0 | 2830 | 836 | 836 | 0.0 | 0.0039 | 0 | 0 | 635861 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha/history | 31 | 1 | 28 | 0 | 2967 | 604 | 948 | 0.0 | 0.0000 | 0 | 0 | 726878 |
| High | admin | logged-in-admin | mobile | /vi/admin/deposits | 31 | 1 | 28 | 0 | 3136 | 568 | 892 | 0.0 | 0.0000 | 0 | 0 | 650210 |
| High | admin | logged-in-admin | mobile | /vi/admin/readings | 31 | 1 | 28 | 0 | 3319 | 668 | 1000 | 0.0 | 0.0000 | 0 | 0 | 651389 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha/history | 31 | 1 | 28 | 0 | 2944 | 556 | 896 | 0.0 | 0.0000 | 0 | 0 | 725270 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit | 30 | 2 | 26 | 0 | 3002 | 612 | 1032 | 0.0 | 0.0039 | 0 | 0 | 634089 |
| High | admin | logged-in-admin | desktop | /vi/admin/deposits | 30 | 0 | 28 | 0 | 2924 | 912 | 948 | 0.0 | 0.0000 | 0 | 0 | 649409 |
| High | admin | logged-in-admin | desktop | /vi/admin/system-configs | 30 | 0 | 28 | 0 | 2978 | 912 | 972 | 0.0 | 0.0000 | 0 | 0 | 689626 |
| High | admin | logged-in-admin | desktop | /vi/admin/users | 30 | 0 | 28 | 0 | 2923 | 828 | 1180 | 0.0 | 0.0000 | 0 | 0 | 649933 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers | 30 | 1 | 27 | 0 | 2906 | 596 | 984 | 0.0 | 0.0039 | 0 | 0 | 636607 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/leaderboard | 30 | 0 | 28 | 0 | 2796 | 692 | 1048 | 0.0 | 0.0177 | 0 | 0 | 649972 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/community | 30 | 0 | 28 | 0 | 3346 | 600 | 1792 | 0.0 | 0.0039 | 0 | 0 | 643509 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/gamification | 30 | 1 | 27 | 0 | 3017 | 696 | 1060 | 0.0 | 0.0190 | 0 | 0 | 644841 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/community | 30 | 0 | 28 | 0 | 3482 | 572 | 1940 | 0.0 | 0.0051 | 0 | 0 | 643174 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 30 | 1 | 27 | 0 | 2866 | 604 | 940 | 0.0 | 0.0000 | 0 | 0 | 644616 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet | 30 | 2 | 26 | 0 | 2916 | 784 | 1120 | 0.0 | 0.0000 | 0 | 0 | 636281 |
| High | admin | logged-in-admin | mobile | /vi/admin/system-configs | 30 | 0 | 28 | 0 | 2866 | 724 | 724 | 0.0 | 0.0000 | 0 | 0 | 689279 |
| High | admin | logged-in-admin | mobile | /vi/admin/users | 30 | 0 | 28 | 0 | 2762 | 560 | 892 | 0.0 | 0.0000 | 0 | 0 | 649997 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers | 30 | 1 | 27 | 0 | 2915 | 560 | 904 | 0.0 | 0.0000 | 0 | 0 | 642031 |
| High | community-leaderboard-quest | logged-in-reader | mobile | /vi/leaderboard | 30 | 0 | 28 | 0 | 2740 | 560 | 892 | 0.0 | 0.0196 | 0 | 0 | 649688 |
| High | community-leaderboard-quest | logged-in-reader | mobile | /vi/community | 30 | 0 | 28 | 0 | 3482 | 636 | 1776 | 0.0 | 0.0051 | 0 | 0 | 643421 |
| High | community-leaderboard-quest | logged-in-reader | mobile | /vi/gamification | 30 | 1 | 27 | 0 | 2873 | 564 | 896 | 0.0 | 0.0000 | 0 | 0 | 645034 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | 29 | 0 | 27 | 0 | 6464 | 692 | 692 | 0.0 | 0.0040 | 0 | 0 | 643136 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet | 29 | 1 | 26 | 0 | 2851 | 624 | 1072 | 0.0 | 0.0039 | 0 | 0 | 635390 |
| High | admin | logged-in-admin | desktop | /vi/admin | 29 | 0 | 27 | 0 | 3306 | 924 | 924 | 0.0 | 0.0000 | 0 | 0 | 647668 |
| High | admin | logged-in-admin | desktop | /vi/admin/disputes | 29 | 0 | 27 | 0 | 2902 | 660 | 968 | 0.0 | 0.0000 | 0 | 0 | 646148 |
| High | admin | logged-in-admin | desktop | /vi/admin/reader-requests | 29 | 0 | 27 | 0 | 2869 | 940 | 940 | 0.0 | 0.0000 | 0 | 0 | 646671 |
| High | admin | logged-in-admin | desktop | /vi/admin/readings | 29 | 0 | 27 | 0 | 2920 | 768 | 1160 | 0.0 | 0.0000 | 0 | 0 | 648927 |
| High | admin | logged-in-admin | desktop | /vi/admin/withdrawals | 29 | 0 | 27 | 0 | 2886 | 900 | 900 | 0.0 | 0.0000 | 0 | 0 | 646403 |
| High | reading | logged-in-reader | desktop | /vi/reading | 29 | 1 | 26 | 0 | 2948 | 608 | 980 | 0.0 | 0.0039 | 0 | 0 | 642976 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | 29 | 0 | 27 | 0 | 6280 | 620 | 620 | 0.0 | 0.0040 | 0 | 0 | 642111 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet | 29 | 1 | 26 | 0 | 2888 | 700 | 1220 | 0.0 | 0.0039 | 0 | 0 | 635299 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/withdraw | 29 | 1 | 26 | 0 | 2886 | 648 | 1036 | 0.0 | 0.0095 | 0 | 0 | 634326 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/notifications | 29 | 1 | 26 | 0 | 2814 | 616 | 988 | 0.0 | 0.0040 | 0 | 0 | 633316 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | 29 | 0 | 27 | 0 | 5533 | 528 | 528 | 0.0 | 0.0000 | 0 | 0 | 643033 |
| High | admin | logged-in-admin | mobile | /vi/admin | 29 | 0 | 27 | 0 | 2941 | 612 | 928 | 0.0 | 0.0000 | 0 | 0 | 647607 |
| High | admin | logged-in-admin | mobile | /vi/admin/disputes | 29 | 0 | 27 | 0 | 2805 | 604 | 920 | 0.0 | 0.0000 | 0 | 0 | 645897 |
| High | admin | logged-in-admin | mobile | /vi/admin/reader-requests | 29 | 0 | 27 | 0 | 3176 | 748 | 1072 | 0.0 | 0.0000 | 0 | 0 | 646573 |
| High | admin | logged-in-admin | mobile | /vi/admin/withdrawals | 29 | 0 | 27 | 0 | 2837 | 556 | 864 | 0.0 | 0.0000 | 0 | 0 | 646200 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | 29 | 0 | 27 | 0 | 5567 | 572 | 572 | 0.0 | 0.0000 | 0 | 0 | 642449 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit | 29 | 1 | 26 | 0 | 2920 | 560 | 904 | 0.0 | 0.0000 | 0 | 0 | 632848 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/notifications | 29 | 1 | 26 | 0 | 2897 | 556 | 888 | 0.0 | 0.0000 | 0 | 0 | 633357 |
| High | reading | logged-in-admin | desktop | /vi/reading | 28 | 0 | 26 | 0 | 2864 | 692 | 1084 | 0.0 | 0.0039 | 0 | 0 | 641979 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | 28 | 0 | 26 | 0 | 2882 | 812 | 1176 | 0.0 | 0.0039 | 0 | 0 | 631428 |
| High | reader-chat | logged-in-admin | desktop | /vi/chat | 28 | 0 | 26 | 0 | 2845 | 624 | 1024 | 0.0 | 0.0039 | 0 | 0 | 631583 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 2862 | 616 | 1128 | 0.0 | 0.0039 | 0 | 0 | 632632 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | 28 | 0 | 26 | 0 | 2897 | 680 | 1192 | 0.0 | 0.0039 | 0 | 0 | 631088 |
| High | reader-chat | logged-in-admin | desktop | /vi/reader/apply | 28 | 0 | 26 | 0 | 2835 | 748 | 1108 | 0.0 | 0.0039 | 0 | 0 | 632863 |
| High | auth-public | logged-in-admin | desktop | /vi/legal/privacy | 28 | 1 | 24 | 0 | 3089 | 608 | 904 | 0.0 | 0.0019 | 0 | 0 | 529277 |
| High | admin | logged-in-admin | desktop | /vi/admin/promotions | 28 | 0 | 26 | 0 | 2846 | 864 | 864 | 0.0 | 0.0000 | 0 | 0 | 644758 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/babb4901-7efc-4608-9c2a-2f440593355a | 28 | 0 | 26 | 0 | 2777 | 648 | 1028 | 0.0 | 0.0039 | 0 | 0 | 632303 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/f8120485-4229-4f7a-940d-177a8c493b86 | 28 | 0 | 26 | 0 | 2895 | 844 | 1200 | 0.0 | 0.0039 | 0 | 0 | 632698 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 2870 | 716 | 1064 | 0.0 | 0.0039 | 0 | 0 | 631626 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 2885 | 688 | 1036 | 0.0 | 0.0039 | 0 | 0 | 631111 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 2760 | 692 | 1048 | 0.0 | 0.0039 | 0 | 0 | 631005 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 2772 | 648 | 1176 | 0.0 | 0.0039 | 0 | 0 | 633116 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 2890 | 740 | 1216 | 0.0 | 0.0039 | 0 | 0 | 633239 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/mfa | 28 | 0 | 26 | 0 | 2771 | 608 | 964 | 0.0 | 0.0039 | 0 | 0 | 631701 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/reader | 28 | 0 | 26 | 0 | 3076 | 588 | 976 | 0.0 | 0.0039 | 0 | 0 | 632287 |
| High | reader-chat | logged-in-reader | desktop | /vi/chat | 28 | 0 | 26 | 0 | 2788 | 660 | 1028 | 0.0 | 0.0039 | 0 | 0 | 632056 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 2749 | 616 | 1012 | 0.0 | 0.0039 | 0 | 0 | 632273 |
| High | reading | logged-in-reader | desktop | /vi/reading/history | 28 | 0 | 26 | 0 | 2690 | 564 | 1052 | 0.0 | 0.0039 | 0 | 0 | 633142 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/2df0d894-36ec-4c7f-8504-0214c910d622 | 28 | 0 | 26 | 0 | 2754 | 576 | 916 | 0.0 | 0.0039 | 0 | 0 | 632474 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/b1566634-eec7-4757-aa30-6b2551343a3e | 28 | 0 | 26 | 0 | 2778 | 620 | 988 | 0.0 | 0.0039 | 0 | 0 | 632528 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/2e1d559a-b66e-4284-a674-a724ac8db4bc | 28 | 0 | 26 | 0 | 2693 | 588 | 972 | 0.0 | 0.0039 | 0 | 0 | 632354 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/6e93c793-9c57-497b-b4e8-6472f2ceab98 | 28 | 0 | 26 | 0 | 2729 | 576 | 944 | 0.0 | 0.0039 | 0 | 0 | 632618 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 2900 | 644 | 1048 | 0.0 | 0.0039 | 0 | 0 | 631560 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 2750 | 696 | 1068 | 0.0 | 0.0039 | 0 | 0 | 631517 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 2691 | 604 | 1044 | 0.0 | 0.0039 | 0 | 0 | 633147 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 2745 | 576 | 1112 | 0.0 | 0.0039 | 0 | 0 | 633168 |
| High | reading | logged-in-admin | mobile | /vi/reading | 28 | 0 | 26 | 0 | 3290 | 1064 | 1404 | 0.0 | 0.0000 | 0 | 0 | 641733 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/mfa | 28 | 0 | 26 | 0 | 2736 | 564 | 904 | 0.0 | 0.0000 | 0 | 0 | 631679 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | 28 | 0 | 26 | 0 | 2767 | 724 | 724 | 0.0 | 0.0712 | 0 | 0 | 630936 |
| High | reader-chat | logged-in-admin | mobile | /vi/chat | 28 | 0 | 26 | 0 | 2731 | 560 | 888 | 0.0 | 0.0000 | 0 | 0 | 631710 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 2999 | 660 | 1000 | 0.0 | 0.0000 | 0 | 0 | 631810 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 2941 | 708 | 1036 | 0.0 | 0.0000 | 0 | 0 | 632378 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 28 | 0 | 26 | 0 | 2692 | 668 | 1008 | 0.0 | 0.0069 | 0 | 0 | 630870 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/notifications | 28 | 0 | 26 | 0 | 2673 | 548 | 888 | 0.0 | 0.0000 | 0 | 0 | 632013 |
| High | reader-chat | logged-in-admin | mobile | /vi/reader/apply | 28 | 0 | 26 | 0 | 3323 | 1140 | 1468 | 0.0 | 0.0000 | 0 | 0 | 632518 |
| High | reading | logged-in-admin | mobile | /vi/reading/history | 28 | 0 | 26 | 0 | 2796 | 580 | 908 | 0.0 | 0.0000 | 0 | 0 | 632846 |
| High | auth-public | logged-in-admin | mobile | /vi/legal/privacy | 28 | 1 | 24 | 0 | 3043 | 588 | 1020 | 0.0 | 0.0028 | 0 | 0 | 529968 |
| High | admin | logged-in-admin | mobile | /vi/admin/promotions | 28 | 0 | 26 | 0 | 2874 | 632 | 944 | 0.0 | 0.0000 | 0 | 0 | 644790 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/b601df3a-c033-40f9-94f7-fe7326b26ede | 28 | 0 | 26 | 0 | 2807 | 576 | 916 | 0.0 | 0.0000 | 0 | 0 | 632466 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/6f3411e2-e177-4e3e-9ff0-73cd67d7c8ab | 28 | 0 | 26 | 0 | 2932 | 724 | 1072 | 0.0 | 0.0000 | 0 | 0 | 632481 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/88b356a6-9950-4682-9c9b-191c0d5831df | 28 | 0 | 26 | 0 | 2736 | 552 | 896 | 0.0 | 0.0000 | 0 | 0 | 632216 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 2800 | 584 | 904 | 0.0 | 0.0000 | 0 | 0 | 631217 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 2752 | 564 | 900 | 0.0 | 0.0000 | 0 | 0 | 631308 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 3211 | 580 | 924 | 0.0 | 0.0069 | 0 | 0 | 631532 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 3431 | 1248 | 1584 | 0.0 | 0.0000 | 0 | 0 | 632775 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 2909 | 744 | 1152 | 0.0 | 0.0000 | 0 | 0 | 632952 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 2792 | 572 | 912 | 0.0 | 0.0000 | 0 | 0 | 633270 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/mfa | 28 | 0 | 26 | 0 | 2853 | 812 | 812 | 0.0 | 0.0000 | 0 | 0 | 631887 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/reader | 28 | 0 | 26 | 0 | 2779 | 576 | 1324 | 0.0 | 0.0000 | 0 | 0 | 632518 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet | 28 | 0 | 26 | 0 | 2930 | 576 | 992 | 0.0 | 0.0000 | 0 | 0 | 634257 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 3033 | 612 | 948 | 0.0 | 0.0000 | 0 | 0 | 632011 |
| High | reader-chat | logged-in-reader | mobile | /vi/reader/apply | 28 | 0 | 26 | 0 | 2783 | 568 | 896 | 0.0 | 0.0000 | 0 | 0 | 632851 |
| High | reading | logged-in-reader | mobile | /vi/reading/history | 28 | 0 | 26 | 0 | 2809 | 556 | 980 | 0.0 | 0.0000 | 0 | 0 | 633329 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/8560e503-b2e2-441b-902c-d1ca24570b99 | 28 | 0 | 26 | 0 | 2955 | 676 | 1008 | 0.0 | 0.0000 | 0 | 0 | 632490 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/3a1a8539-ac7d-4b3c-830c-4d82a830739d | 28 | 0 | 26 | 0 | 2880 | 676 | 1008 | 0.0 | 0.0000 | 0 | 0 | 632283 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 2835 | 708 | 1036 | 0.0 | 0.0000 | 0 | 0 | 631593 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 2713 | 548 | 880 | 0.0 | 0.0000 | 0 | 0 | 631368 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 2723 | 560 | 880 | 0.0 | 0.0000 | 0 | 0 | 631166 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 2760 | 556 | 968 | 0.0 | 0.0000 | 0 | 0 | 632860 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 2783 | 580 | 984 | 0.0 | 0.0000 | 0 | 0 | 633164 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 2874 | 604 | 1008 | 0.0 | 0.0000 | 0 | 0 | 633684 |
| High | auth-public | logged-in-admin | desktop | /vi | 26 | 0 | 24 | 0 | 2734 | 688 | 1184 | 135.0 | 0.0035 | 0 | 0 | 537608 |
| High | auth-public | logged-in-reader | desktop | /vi | 26 | 0 | 24 | 0 | 2724 | 628 | 1152 | 159.0 | 0.0033 | 0 | 0 | 537810 |
| High | auth-public | logged-in-admin | mobile | /vi | 26 | 0 | 24 | 0 | 2776 | 632 | 984 | 0.0 | 0.0028 | 0 | 0 | 537720 |
| High | auth-public | logged-in-reader | mobile | /vi | 26 | 0 | 24 | 0 | 2922 | 684 | 1036 | 0.0 | 0.0032 | 0 | 0 | 537415 |
| High | auth-public | logged-out | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 3133 | 752 | 752 | 0.0 | 0.0000 | 0 | 0 | 525849 |
| High | auth-public | logged-in-reader | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 3417 | 668 | 964 | 0.0 | 0.0019 | 0 | 0 | 526394 |
| High | auth-public | logged-out | mobile | /vi/register | 25 | 1 | 22 | 0 | 3773 | 1452 | 1452 | 0.0 | 0.0000 | 0 | 0 | 513688 |
| High | auth-public | logged-out | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 3426 | 1092 | 1092 | 0.0 | 0.0000 | 0 | 0 | 525958 |
| High | auth-public | logged-out | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 3480 | 1088 | 1088 | 0.0 | 0.0000 | 0 | 0 | 525978 |
| High | auth-public | logged-out | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 3843 | 1692 | 2008 | 0.0 | 0.0000 | 0 | 0 | 526115 |
| High | auth-public | logged-out | mobile | /vi/login | 24 | 0 | 22 | 0 | 3462 | 1452 | 1452 | 0.0 | 0.0000 | 0 | 0 | 512411 |
| High | auth-public | logged-out | mobile | /vi/forgot-password | 24 | 0 | 22 | 0 | 4165 | 2156 | 2156 | 0.0 | 0.0000 | 0 | 0 | 512010 |
| High | auth-public | logged-out | mobile | /vi/reset-password | 24 | 0 | 22 | 0 | 3341 | 1164 | 1164 | 0.0 | 0.0000 | 0 | 0 | 512060 |
| High | auth-public | logged-out | mobile | /vi/verify-email | 24 | 0 | 22 | 0 | 3688 | 1128 | 1128 | 0.0 | 0.0000 | 0 | 0 | 512240 |
| High | auth-public | logged-in-admin | mobile | /vi/verify-email | 24 | 0 | 22 | 0 | 2916 | 828 | 828 | 0.0 | 0.0000 | 0 | 0 | 512234 |
| Medium | auth-public | logged-out | desktop | /vi/register | 25 | 1 | 22 | 0 | 2728 | 620 | 620 | 0.0 | 0.0000 | 0 | 0 | 513621 |
| Medium | auth-public | logged-out | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 3049 | 1012 | 1012 | 0.0 | 0.0000 | 0 | 0 | 525913 |
| Medium | auth-public | logged-out | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2680 | 592 | 592 | 0.0 | 0.0000 | 0 | 0 | 526206 |
| Medium | auth-public | logged-in-admin | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 2811 | 624 | 956 | 0.0 | 0.0019 | 0 | 0 | 526357 |
| Medium | auth-public | logged-in-admin | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2744 | 600 | 932 | 0.0 | 0.0019 | 0 | 0 | 526478 |
| Medium | auth-public | logged-in-reader | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 3116 | 756 | 756 | 0.0 | 0.0019 | 0 | 0 | 526510 |
| Medium | auth-public | logged-in-reader | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2829 | 596 | 924 | 0.0 | 0.0019 | 0 | 0 | 526521 |
| Medium | auth-public | logged-in-admin | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 2672 | 540 | 852 | 0.0 | 0.0028 | 0 | 0 | 526197 |
| Medium | auth-public | logged-in-admin | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2837 | 600 | 932 | 0.0 | 0.0028 | 0 | 0 | 526466 |
| Medium | auth-public | logged-in-reader | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 2672 | 548 | 860 | 0.0 | 0.0032 | 0 | 0 | 526373 |
| Medium | auth-public | logged-in-reader | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2732 | 568 | 876 | 0.0 | 0.0032 | 0 | 0 | 526120 |
| Medium | auth-public | logged-in-reader | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2746 | 580 | 892 | 0.0 | 0.0032 | 0 | 0 | 526344 |
| Medium | auth-public | logged-out | desktop | /vi/login | 24 | 0 | 22 | 0 | 2715 | 636 | 636 | 0.0 | 0.0000 | 0 | 0 | 512367 |
| Medium | auth-public | logged-out | desktop | /vi/forgot-password | 24 | 0 | 22 | 0 | 2654 | 576 | 576 | 0.0 | 0.0000 | 0 | 0 | 512038 |
| Medium | auth-public | logged-out | desktop | /vi/reset-password | 24 | 0 | 22 | 0 | 2702 | 608 | 608 | 0.0 | 0.0000 | 0 | 0 | 512122 |
| Medium | auth-public | logged-out | desktop | /vi/verify-email | 24 | 0 | 22 | 0 | 2981 | 752 | 752 | 0.0 | 0.0000 | 0 | 0 | 512231 |
| Medium | auth-public | logged-in-admin | desktop | /vi/reset-password | 24 | 0 | 22 | 0 | 2710 | 692 | 692 | 0.0 | 0.0000 | 0 | 0 | 512132 |
| Medium | auth-public | logged-in-admin | desktop | /vi/verify-email | 24 | 0 | 22 | 0 | 2640 | 652 | 652 | 0.0 | 0.0000 | 0 | 0 | 512147 |
| Medium | auth-public | logged-in-reader | desktop | /vi/reset-password | 24 | 0 | 22 | 0 | 2692 | 640 | 640 | 0.0 | 0.0000 | 0 | 0 | 512076 |
| Medium | auth-public | logged-in-reader | desktop | /vi/verify-email | 24 | 0 | 22 | 0 | 2774 | 672 | 672 | 0.0 | 0.0000 | 0 | 0 | 512241 |
| Medium | auth-public | logged-in-admin | mobile | /vi/reset-password | 24 | 0 | 22 | 0 | 2687 | 592 | 592 | 0.0 | 0.0000 | 0 | 0 | 512150 |
| Medium | auth-public | logged-in-reader | mobile | /vi/reset-password | 24 | 0 | 22 | 0 | 2662 | 556 | 556 | 0.0 | 0.0000 | 0 | 0 | 512089 |
| Medium | auth-public | logged-in-reader | mobile | /vi/verify-email | 24 | 0 | 22 | 0 | 2688 | 552 | 552 | 0.0 | 0.0000 | 0 | 0 | 512156 |

## Major Issues Found

- Critical: 34 page(s) có request count >35, pending request, failed request, hoặc issue nghiêm trọng.
- High: 139 page(s) vượt ngưỡng >25 requests hoặc có request >800ms.
- Medium: 23 page(s) có request trong dải 400-800ms.
- Duplicate: 305 nhóm duplicate request cần kiểm tra over-fetch/cache key.
- Pending: 5 page(s) có pending request không phải websocket/eventsource.

### Slow Requests

| Severity | Feature | Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 2919 | 1862 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F15_Temperance_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | reader-chat | logged-in-reader | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 2728 | 313 | html | https://www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 2522 | 1775 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F13_The_Hanged_Man_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 2460 | 1435 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F04_The_Empress_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | reading | logged-in-admin | desktop | /vi/reading/history | GET | 200 | 2353 | 331 | html | https://www.tarotnow.xyz/vi/reading/history |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 2136 | 1406 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F03_The_High+Priestess+_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | auth-public | logged-out | desktop | /vi | GET | 200 | 2074 | 908 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | auth-public | logged-out | mobile | /vi | GET | 200 | 1942 | 1940 | api | https://www.tarotnow.xyz/api/readers?page=1&pageSize=4 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1877 | 171 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F11_Wheel_of+_Fortune_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | auth-public | logged-out | mobile | /vi/forgot-password | GET | 200 | 1869 | 1461 | html | https://www.tarotnow.xyz/vi/forgot-password |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1864 | 126 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F04_The_Empress_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1719 | 253 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F12_Justice_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1710 | 1140 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F16_The_Devil_50_20260325_181357.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | auth-public | logged-out | mobile | /vi/register | GET | 200 | 1701 | 562 | html | https://www.tarotnow.xyz/vi/register |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1701 | 219 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F01_The_Fool_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1658 | 150 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F07_The_Lovers_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1636 | 139 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F06_The_Hierophant_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1627 | 206 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F09_Strength_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1626 | 301 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F15_Temperance_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | reader-chat | logged-in-reader | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 1591 | 1587 | api | https://www.tarotnow.xyz/api/auth/session |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1448 | 306 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F02_The_Magician_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 1350 | 998 | html | https://www.tarotnow.xyz/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/community | GET | 200 | 1251 | 782 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/reader/apply | GET | 200 | 1242 | 912 | html | https://www.tarotnow.xyz/vi/reader/apply |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1224 | 257 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F13_The_Hanged_Man_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | reading | logged-in-admin | mobile | /vi/reading | GET | 200 | 1220 | 823 | html | https://www.tarotnow.xyz/vi/reading |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/2aee7a1c-4322-427e-b5c1-30972072e89b | GET | 200 | 1212 | 854 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | reading | logged-in-admin | desktop | /vi/reading/history | GET | 200 | 1188 | 1185 | api | https://www.tarotnow.xyz/api/auth/session |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1177 | 112 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F11_Wheel_of+_Fortune_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1171 | 290 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F04_The_Empress_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1166 | 313 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F03_The_High+Priestess+_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1163 | 313 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F03_The_High+Priestess+_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/eb1f6399-08c3-4a62-ac47-1989f2a4a680 | GET | 200 | 1157 | 1144 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | auth-public | logged-out | mobile | /vi/verify-email | GET | 200 | 1148 | 549 | html | https://www.tarotnow.xyz/vi/verify-email |
| Critical | auth-public | logged-out | mobile | /vi/legal/ai-disclaimer | GET | 200 | 1148 | 1115 | html | https://www.tarotnow.xyz/vi/legal/ai-disclaimer |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1085 | 110 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F06_The_Hierophant_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | auth-public | logged-out | desktop | /vi | GET | 200 | 1079 | 1071 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=384&q=75 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | GET | 200 | 1079 | 332 | html | https://www.tarotnow.xyz/vi/profile |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1077 | 328 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F14_Death_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | auth-public | logged-out | desktop | /vi | GET | 200 | 1072 | 545 | static | https://www.tarotnow.xyz/_next/static/chunks/0e-efl~gmt6zm.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/legal/tos | GET | 200 | 1065 | 1052 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | auth-public | logged-out | mobile | /vi/verify-email | GET | 200 | 1063 | 409 | static | https://www.tarotnow.xyz/_next/static/chunks/0e-efl~gmt6zm.js |
| Critical | auth-public | logged-out | mobile | /vi/verify-email | GET | 200 | 1061 | 503 | static | https://www.tarotnow.xyz/_next/static/chunks/07c1epgcg_nj_.js |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1060 | 133 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F02_The_Magician_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | auth-public | logged-out | desktop | /vi | GET | 200 | 1059 | 544 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1059 | 127 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F12_Justice_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | auth-public | logged-out | mobile | /vi | GET | 200 | 1057 | 604 | html | https://www.tarotnow.xyz/vi |
| Critical | auth-public | logged-out | desktop | /vi | GET | 200 | 1050 | 578 | html | https://www.tarotnow.xyz/vi |
| Critical | auth-public | logged-out | desktop | /vi/legal/privacy | GET | 200 | 1050 | 356 | html | https://www.tarotnow.xyz/vi/legal/privacy |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1040 | 100 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F07_The_Lovers_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | GET | 200 | 1024 | 1020 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Frare_title_lucky_star_50_20260416_165453.avif&w=48&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1024 | 206 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F08_The_Chariot_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1022 | 99 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F01_The_Fool_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1019 | 126 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F05_The_Emperor_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1011 | 300 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F14_Death_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1009 | 100 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F09_Strength_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | community-leaderboard-quest | logged-in-admin | mobile | /vi/community | GET | 200 | 1004 | 514 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1200&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 994 | 96 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F02_The_Magician_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 984 | 318 | html | https://www.tarotnow.xyz/vi/login |
| Critical | auth-public | logged-in-admin | mobile | /vi/legal/privacy | GET | 200 | 971 | 349 | html | https://www.tarotnow.xyz/vi/legal/privacy |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 966 | 103 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F05_The_Emperor_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | auth-public | logged-out | mobile | /vi/legal/tos | GET | 200 | 966 | 552 | html | https://www.tarotnow.xyz/vi/legal/tos |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 962 | 170 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F01_The_Fool_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | GET | 200 | 961 | 956 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Fpower_booster_50_20260416_165453.avif&w=48&q=75 |
| Critical | auth-public | logged-out | mobile | /vi/reset-password | GET | 200 | 960 | 603 | html | https://www.tarotnow.xyz/vi/reset-password |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | GET | 200 | 955 | 949 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Fexp_booster_50_20260416_165452.avif&w=48&q=75 |
| Critical | auth-public | logged-out | mobile | /vi | GET | 401 | 951 | 947 | api | https://www.tarotnow.xyz/api/me/runtime-policies |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha | GET | 200 | 945 | 702 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Frare_title_lucky_star_50_20260416_165453.avif&w=256&q=75 |
| Critical | auth-public | logged-out | mobile | /vi/legal/privacy | GET | 200 | 921 | 602 | html | https://www.tarotnow.xyz/vi/legal/privacy |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | GET | 200 | 915 | 913 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Fdefense_booster_50_20260416_165452.avif&w=48&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | GET | 200 | 914 | 910 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Ffree_draw_ticket_50_20260416_165452.avif&w=48&q=75 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit | GET | 200 | 911 | 340 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha | GET | 200 | 900 | 642 | static | https://www.tarotnow.xyz/_next/static/chunks/0to-xfrh5ita1.js |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 900 | 259 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F16_The_Devil_50_20260325_181357.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | auth-public | logged-out | mobile | /vi/login | GET | 200 | 898 | 572 | html | https://www.tarotnow.xyz/vi/login |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit | GET | 200 | 897 | 331 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | GET | 200 | 895 | 203 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 895 | 76 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F01_The_Fool_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | admin | logged-in-admin | mobile | /vi/admin/readings | GET | 200 | 895 | 750 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | auth-public | logged-out | mobile | /vi/verify-email | GET | 200 | 887 | 365 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |

### Duplicate API / Request Candidates

| Severity | Feature | Scenario | Viewport | Route | Count | Request Key |
| --- | --- | --- | --- | --- | ---: | --- |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/themes/prismatic-royal.css |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0q8qdu-5ybfjk.js |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0e-efl~gmt6zm.js |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/07c1epgcg_nj_.js |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/themes/prismatic-royal.css |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0q8qdu-5ybfjk.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0e-efl~gmt6zm.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/07c1epgcg_nj_.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/themes/prismatic-royal.css |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |

### Pending Requests

| Severity | Feature | Scenario | Viewport | Route | URL |
| --- | --- | --- | --- | --- | --- |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/api/readers?page=1&pageSize=4 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=384&q=75 |
| Critical | auth-public | logged-out | mobile | /vi | https://www.tarotnow.xyz/api/me/runtime-policies |
| Critical | auth-public | logged-out | mobile | /vi | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=1200&q=75 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/chat | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=1200&q=75 |

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
