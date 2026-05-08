# PERFORMANCE AUDIT - TarotNow

## Executive Summary

- Generated at (UTC): 2026-05-08T20:32:24.734Z
- Benchmark generated at (UTC): 2026-05-08T20:32:14.963Z
- Benchmark input: Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark.json
- Base origin: https://www.tarotnow.xyz
- Locale prefix: /vi
- Benchmark mode: full-matrix
- Total scenarios: 6
- Total pages measured: 170
- Critical pages: 0
- High pages: 147
- Medium pages: 23
- Slow requests >800ms: 136
- Slow requests 400-800ms: 493
- Request thresholds: >25 suspicious, >35 severe
- Slow request thresholds: >400ms optimize, >800ms serious

## Scenario Coverage

| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Failed | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 24.9 | 2833 | 0 | 0 | yes |
| logged-in-admin | desktop | 43 | 28.7 | 2950 | 0 | 0 | yes |
| logged-in-reader | desktop | 33 | 28.7 | 3077 | 0 | 0 | yes |
| logged-out | mobile | 9 | 24.9 | 2828 | 0 | 0 | yes |
| logged-in-admin | mobile | 43 | 29.2 | 3070 | 0 | 0 | yes |
| logged-in-reader | mobile | 33 | 29.1 | 3318 | 0 | 0 | yes |

## Route Family Coverage

| Scenario | Viewport | Family | Pages | Avg requests/page | Avg nav (ms) | Pending | Failed |
| --- | --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.4 | 2860 | 0 | 0 |
| logged-in-admin | desktop | auth-public | 3 | 25.0 | 2691 | 0 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | 3 | 29.7 | 3156 | 0 | 0 |
| logged-in-admin | desktop | home | 1 | 26.0 | 2743 | 0 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | 4 | 30.5 | 3851 | 0 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | 8 | 28.5 | 2838 | 0 | 0 |
| logged-in-admin | desktop | reader-chat | 9 | 28.2 | 2833 | 0 | 0 |
| logged-in-admin | desktop | reading | 5 | 29.4 | 2868 | 0 | 0 |
| logged-in-admin | mobile | admin | 10 | 29.5 | 2936 | 0 | 0 |
| logged-in-admin | mobile | auth-public | 3 | 25.7 | 3556 | 0 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | 3 | 32.0 | 3178 | 0 | 0 |
| logged-in-admin | mobile | home | 1 | 26.0 | 2752 | 0 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | 4 | 30.8 | 3705 | 0 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | 8 | 28.5 | 2867 | 0 | 0 |
| logged-in-admin | mobile | reader-chat | 9 | 28.4 | 2952 | 0 | 0 |
| logged-in-admin | mobile | reading | 5 | 30.6 | 3076 | 0 | 0 |
| logged-in-reader | desktop | auth-public | 3 | 25.0 | 3050 | 0 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | 3 | 29.7 | 3204 | 0 | 0 |
| logged-in-reader | desktop | home | 1 | 26.0 | 2759 | 0 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | 4 | 30.8 | 3894 | 0 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | 8 | 28.8 | 2945 | 0 | 0 |
| logged-in-reader | desktop | reader-chat | 9 | 28.3 | 2837 | 0 | 0 |
| logged-in-reader | desktop | reading | 5 | 29.6 | 3070 | 0 | 0 |
| logged-in-reader | mobile | auth-public | 3 | 25.0 | 3489 | 0 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | 3 | 30.0 | 3315 | 0 | 0 |
| logged-in-reader | mobile | home | 1 | 34.0 | 3427 | 0 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | 4 | 30.3 | 3792 | 0 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | 8 | 29.0 | 3340 | 0 | 0 |
| logged-in-reader | mobile | reader-chat | 9 | 28.0 | 3002 | 0 | 0 |
| logged-in-reader | mobile | reading | 5 | 31.0 | 3348 | 0 | 0 |
| logged-out | desktop | auth-public | 8 | 24.4 | 2739 | 0 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 3584 | 0 | 0 |
| logged-out | mobile | auth-public | 8 | 24.4 | 2761 | 0 | 0 |
| logged-out | mobile | home | 1 | 29.0 | 3364 | 0 | 0 |

## Detailed Metrics Table

| Severity | Feature | Scenario | Viewport | Route | Requests | API | Static | Third-party | Nav (ms) | FCP (ms) | LCP (ms) | TBT (ms) | CLS | Pending | Failed | Transfer bytes |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/community | 35 | 1 | 32 | 0 | 3734 | 480 | 1672 | 0.0 | 0.0051 | 0 | 0 | 776467 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/275c6df1-5a67-445b-a9db-b1d5ba316b23 | 34 | 2 | 30 | 0 | 3098 | 568 | 1036 | 0.0 | 0.0041 | 0 | 0 | 724930 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/bfb8919e-c739-44f7-8934-63cd5dbeb9e4 | 34 | 2 | 30 | 0 | 2996 | 536 | 924 | 0.0 | 0.0039 | 0 | 0 | 724683 |
| High | auth-public | logged-in-reader | mobile | /vi | 34 | 4 | 27 | 0 | 3427 | 912 | 912 | 2.0 | 0.0032 | 0 | 0 | 611090 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/ad085b60-8b51-43a6-9018-4217d2c3914d | 33 | 2 | 29 | 0 | 2972 | 452 | 804 | 0.0 | 0.0000 | 0 | 0 | 692961 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/09ef272e-4364-4d56-82ba-498ed1c6ead7 | 33 | 2 | 29 | 0 | 2977 | 468 | 804 | 0.0 | 0.0000 | 0 | 0 | 692978 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/cba5eb9d-ff2a-41c5-8b76-9bb5bcdc0989 | 33 | 2 | 29 | 0 | 3155 | 508 | 860 | 0.0 | 0.0001 | 0 | 0 | 692831 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/16a7bb79-f7ea-4f53-8dbc-c5250273cddc | 33 | 2 | 29 | 0 | 3037 | 492 | 860 | 0.0 | 0.0000 | 0 | 0 | 692781 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha | 32 | 2 | 28 | 0 | 2914 | 512 | 1236 | 0.0 | 0.0041 | 0 | 0 | 726067 |
| High | admin | logged-in-admin | desktop | /vi/admin/gamification | 32 | 0 | 30 | 0 | 2997 | 532 | 908 | 0.0 | 0.0022 | 0 | 0 | 697982 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha | 32 | 2 | 28 | 0 | 2916 | 544 | 1232 | 0.0 | 0.0039 | 0 | 0 | 726257 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha/history | 32 | 2 | 28 | 0 | 2914 | 580 | 1392 | 0.0 | 0.0039 | 0 | 0 | 725383 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/inventory | 32 | 2 | 28 | 0 | 2853 | 452 | 1080 | 0.0 | 0.0000 | 0 | 0 | 645811 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha | 32 | 2 | 28 | 0 | 2845 | 436 | 1084 | 0.0 | 0.0000 | 0 | 0 | 725960 |
| High | admin | logged-in-admin | mobile | /vi/admin/gamification | 32 | 0 | 30 | 0 | 3031 | 440 | 816 | 0.0 | 0.0000 | 0 | 0 | 697990 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | 31 | 1 | 28 | 0 | 2968 | 600 | 1284 | 0.0 | 0.0041 | 0 | 0 | 644640 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile | 31 | 2 | 27 | 0 | 2978 | 552 | 1216 | 0.0 | 0.0489 | 0 | 0 | 635553 |
| High | admin | logged-in-admin | desktop | /vi/admin/users | 31 | 0 | 29 | 0 | 2899 | 592 | 1216 | 0.0 | 0.0000 | 0 | 0 | 651088 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile | 31 | 2 | 27 | 0 | 2954 | 560 | 1268 | 0.0 | 0.0726 | 0 | 0 | 635521 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/leaderboard | 31 | 1 | 28 | 0 | 2887 | 460 | 1116 | 0.0 | 0.0000 | 0 | 0 | 651025 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/56c81cad-8f33-4a8b-aae2-ea3499e78a3a | 31 | 1 | 28 | 0 | 2911 | 472 | 820 | 0.0 | 0.0000 | 0 | 0 | 681154 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/inventory | 31 | 1 | 28 | 0 | 2997 | 488 | 1156 | 0.0 | 0.0000 | 0 | 0 | 644892 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha/history | 31 | 1 | 28 | 0 | 2930 | 548 | 904 | 0.0 | 0.0000 | 0 | 0 | 724785 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile | 31 | 2 | 27 | 0 | 3037 | 500 | 1140 | 0.0 | 0.0821 | 0 | 0 | 635594 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/withdraw | 31 | 2 | 26 | 0 | 3937 | 928 | 1240 | 0.0 | 0.0330 | 0 | 0 | 634660 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/80e076ca-dfff-469d-804e-1d85cdb0676d | 31 | 1 | 28 | 0 | 3083 | 492 | 840 | 0.0 | 0.0000 | 0 | 0 | 681374 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha/history | 30 | 0 | 28 | 0 | 2914 | 580 | 1228 | 0.0 | 0.0041 | 0 | 0 | 725635 |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/leaderboard | 30 | 0 | 28 | 0 | 2872 | 632 | 1084 | 0.0 | 0.0041 | 0 | 0 | 650815 |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/community | 30 | 0 | 28 | 0 | 3668 | 552 | 1768 | 0.0 | 0.0041 | 0 | 0 | 642640 |
| High | admin | logged-in-admin | desktop | /vi/admin | 30 | 1 | 27 | 0 | 2882 | 516 | 916 | 0.0 | 0.0000 | 0 | 0 | 648368 |
| High | reading | logged-in-reader | desktop | /vi/reading | 30 | 2 | 26 | 0 | 2953 | 524 | 1028 | 0.0 | 0.0039 | 0 | 0 | 643146 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/inventory | 30 | 0 | 28 | 0 | 2901 | 524 | 1196 | 0.0 | 0.0039 | 0 | 0 | 644268 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/leaderboard | 30 | 0 | 28 | 0 | 2907 | 552 | 992 | 0.0 | 0.0039 | 0 | 0 | 650451 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/community | 30 | 0 | 28 | 0 | 3782 | 616 | 1856 | 0.0 | 0.0039 | 0 | 0 | 642787 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha/history | 30 | 0 | 28 | 0 | 2855 | 452 | 836 | 0.0 | 0.0000 | 0 | 0 | 725653 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile | 30 | 1 | 27 | 0 | 3027 | 456 | 1108 | 0.0 | 0.0689 | 0 | 0 | 634315 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 30 | 1 | 27 | 0 | 2912 | 448 | 832 | 0.0 | 0.0000 | 0 | 0 | 642788 |
| High | admin | logged-in-admin | mobile | /vi/admin/deposits | 30 | 0 | 28 | 0 | 2998 | 504 | 1176 | 0.0 | 0.0000 | 0 | 0 | 648759 |
| High | admin | logged-in-admin | mobile | /vi/admin/system-configs | 30 | 0 | 28 | 0 | 2973 | 508 | 896 | 0.0 | 0.0000 | 0 | 0 | 688605 |
| High | admin | logged-in-admin | mobile | /vi/admin/users | 30 | 0 | 28 | 0 | 2947 | 500 | 1140 | 0.0 | 0.0000 | 0 | 0 | 649362 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 30 | 2 | 26 | 0 | 3168 | 488 | 872 | 0.0 | 0.0000 | 0 | 0 | 634272 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha | 30 | 0 | 28 | 0 | 2950 | 468 | 1124 | 0.0 | 0.0000 | 0 | 0 | 724899 |
| High | community-leaderboard-quest | logged-in-reader | mobile | /vi/leaderboard | 30 | 0 | 28 | 0 | 3042 | 508 | 1168 | 0.0 | 0.0000 | 0 | 0 | 650832 |
| High | community-leaderboard-quest | logged-in-reader | mobile | /vi/community | 30 | 0 | 28 | 0 | 3934 | 624 | 2016 | 0.0 | 0.0051 | 0 | 0 | 642765 |
| High | community-leaderboard-quest | logged-in-reader | mobile | /vi/gamification | 30 | 1 | 27 | 0 | 2970 | 504 | 904 | 0.0 | 0.0000 | 0 | 0 | 643335 |
| High | auth-public | logged-out | desktop | /vi | 29 | 0 | 27 | 0 | 3584 | 1412 | 1412 | 387.0 | 0.0000 | 0 | 0 | 600874 |
| High | reading | logged-in-admin | desktop | /vi/reading | 29 | 1 | 26 | 0 | 2935 | 540 | 992 | 0.0 | 0.0041 | 0 | 0 | 642170 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | 29 | 0 | 27 | 0 | 6606 | 532 | 532 | 0.0 | 0.0042 | 0 | 0 | 642680 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers | 29 | 1 | 26 | 0 | 2873 | 528 | 1172 | 0.0 | 0.0041 | 0 | 0 | 634469 |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | 29 | 0 | 27 | 0 | 2928 | 568 | 1044 | 6.0 | 0.0279 | 0 | 0 | 642377 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet | 29 | 1 | 26 | 0 | 2903 | 568 | 1088 | 0.0 | 0.0041 | 0 | 0 | 634589 |
| High | reader-chat | logged-in-admin | desktop | /vi/reader/apply | 29 | 1 | 26 | 0 | 2961 | 548 | 1012 | 0.0 | 0.0041 | 0 | 0 | 632696 |
| High | admin | logged-in-admin | desktop | /vi/admin/deposits | 29 | 0 | 27 | 0 | 2954 | 644 | 1108 | 0.0 | 0.0000 | 0 | 0 | 646965 |
| High | admin | logged-in-admin | desktop | /vi/admin/disputes | 29 | 0 | 27 | 0 | 2875 | 580 | 992 | 0.0 | 0.0000 | 0 | 0 | 645394 |
| High | admin | logged-in-admin | desktop | /vi/admin/reader-requests | 29 | 0 | 27 | 0 | 2812 | 532 | 868 | 0.0 | 0.0000 | 0 | 0 | 646056 |
| High | admin | logged-in-admin | desktop | /vi/admin/readings | 29 | 0 | 27 | 0 | 2889 | 560 | 1120 | 0.0 | 0.0000 | 0 | 0 | 648266 |
| High | admin | logged-in-admin | desktop | /vi/admin/withdrawals | 29 | 0 | 27 | 0 | 2785 | 536 | 900 | 0.0 | 0.0000 | 0 | 0 | 645487 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | 29 | 0 | 27 | 0 | 6845 | 544 | 544 | 12.0 | 0.0040 | 0 | 0 | 641734 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/gamification | 29 | 0 | 27 | 0 | 2922 | 520 | 996 | 0.0 | 0.0277 | 0 | 0 | 641953 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | 29 | 1 | 26 | 0 | 2967 | 580 | 1040 | 0.0 | 0.0039 | 0 | 0 | 631816 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit/history | 29 | 1 | 26 | 0 | 3102 | 640 | 1168 | 0.0 | 0.0039 | 0 | 0 | 631966 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/withdraw | 29 | 1 | 26 | 0 | 2981 | 564 | 1204 | 0.0 | 0.0095 | 0 | 0 | 633084 |
| High | reader-chat | logged-in-reader | desktop | /vi/reader/apply | 29 | 1 | 26 | 0 | 2991 | 620 | 1032 | 0.0 | 0.0039 | 0 | 0 | 632602 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | 29 | 1 | 26 | 0 | 2921 | 580 | 1012 | 0.0 | 0.0039 | 0 | 0 | 631437 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 29 | 1 | 26 | 0 | 2927 | 532 | 988 | 0.0 | 0.0039 | 0 | 0 | 631292 |
| High | auth-public | logged-out | mobile | /vi | 29 | 0 | 27 | 0 | 3364 | 964 | 964 | 0.0 | 0.0000 | 0 | 0 | 602715 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | 29 | 0 | 27 | 0 | 6267 | 512 | 896 | 47.0 | 0.0000 | 0 | 0 | 642717 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/mfa | 29 | 1 | 26 | 0 | 3059 | 484 | 908 | 0.0 | 0.0000 | 0 | 0 | 631628 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet | 29 | 1 | 26 | 0 | 2872 | 448 | 1096 | 0.0 | 0.0000 | 0 | 0 | 634452 |
| High | admin | logged-in-admin | mobile | /vi/admin | 29 | 0 | 27 | 0 | 3037 | 620 | 948 | 0.0 | 0.0000 | 0 | 0 | 647033 |
| High | admin | logged-in-admin | mobile | /vi/admin/disputes | 29 | 0 | 27 | 0 | 2916 | 488 | 800 | 0.0 | 0.0000 | 0 | 0 | 645289 |
| High | admin | logged-in-admin | mobile | /vi/admin/reader-requests | 29 | 0 | 27 | 0 | 2844 | 496 | 820 | 0.0 | 0.0000 | 0 | 0 | 645835 |
| High | admin | logged-in-admin | mobile | /vi/admin/readings | 29 | 0 | 27 | 0 | 2894 | 484 | 856 | 0.0 | 0.0000 | 0 | 0 | 648260 |
| High | admin | logged-in-admin | mobile | /vi/admin/withdrawals | 29 | 0 | 27 | 0 | 2894 | 484 | 804 | 0.0 | 0.0000 | 0 | 0 | 645598 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 29 | 1 | 26 | 0 | 2893 | 472 | 836 | 0.0 | 0.0000 | 0 | 0 | 631383 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 29 | 1 | 26 | 0 | 3004 | 452 | 824 | 0.0 | 0.0000 | 0 | 0 | 632941 |
| High | reading | logged-in-reader | mobile | /vi/reading | 29 | 1 | 26 | 0 | 2949 | 488 | 848 | 0.0 | 0.0000 | 0 | 0 | 641887 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | 29 | 0 | 27 | 0 | 6291 | 840 | 840 | 7.0 | 0.0000 | 0 | 0 | 641787 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet | 29 | 1 | 26 | 0 | 2930 | 484 | 1164 | 0.0 | 0.0000 | 0 | 0 | 634419 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/notifications | 29 | 1 | 26 | 0 | 3829 | 684 | 684 | 0.0 | 0.0000 | 0 | 0 | 632394 |
| High | reading | logged-in-reader | mobile | /vi/reading/history | 29 | 1 | 26 | 0 | 4516 | 900 | 900 | 0.0 | 0.0071 | 0 | 0 | 633035 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | 28 | 0 | 26 | 0 | 2733 | 508 | 940 | 0.0 | 0.0041 | 0 | 0 | 631025 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | 28 | 0 | 26 | 0 | 2722 | 584 | 1216 | 0.0 | 0.0489 | 0 | 0 | 630329 |
| High | reader-chat | logged-in-admin | desktop | /vi/chat | 28 | 0 | 26 | 0 | 2731 | 560 | 988 | 0.0 | 0.0041 | 0 | 0 | 631226 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 2747 | 536 | 880 | 0.0 | 0.0041 | 0 | 0 | 630961 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 2975 | 628 | 1276 | 0.0 | 0.0041 | 0 | 0 | 631790 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | 28 | 0 | 26 | 0 | 2857 | 576 | 1116 | 0.0 | 0.0041 | 0 | 0 | 630483 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/notifications | 28 | 0 | 26 | 0 | 2786 | 556 | 1032 | 0.0 | 0.0041 | 0 | 0 | 631793 |
| High | reading | logged-in-admin | desktop | /vi/reading/history | 28 | 0 | 26 | 0 | 2774 | 532 | 1156 | 0.0 | 0.0041 | 0 | 0 | 632290 |
| High | admin | logged-in-admin | desktop | /vi/admin/promotions | 28 | 0 | 26 | 0 | 2788 | 616 | 940 | 0.0 | 0.0000 | 0 | 0 | 644087 |
| High | admin | logged-in-admin | desktop | /vi/admin/system-configs | 28 | 0 | 26 | 0 | 2720 | 564 | 1060 | 0.0 | 0.0000 | 0 | 0 | 655250 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/698106b6-6bdb-4988-93e6-87034cd28eda | 28 | 0 | 26 | 0 | 2760 | 512 | 944 | 0.0 | 0.0041 | 0 | 0 | 631929 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/1a03a485-ec7d-4912-9cde-416b5bae69f3 | 28 | 0 | 26 | 0 | 2774 | 572 | 968 | 0.0 | 0.0041 | 0 | 0 | 631926 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 2860 | 524 | 936 | 0.0 | 0.0041 | 0 | 0 | 630805 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 2903 | 540 | 952 | 0.0 | 0.0041 | 0 | 0 | 630680 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 2732 | 548 | 980 | 0.0 | 0.0041 | 0 | 0 | 630575 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 2802 | 656 | 1352 | 0.0 | 0.0041 | 0 | 0 | 632450 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 2777 | 580 | 1228 | 0.0 | 0.0041 | 0 | 0 | 632612 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 2862 | 548 | 1228 | 0.0 | 0.0041 | 0 | 0 | 632421 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/mfa | 28 | 0 | 26 | 0 | 2764 | 536 | 924 | 0.0 | 0.0039 | 0 | 0 | 631133 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/reader | 28 | 0 | 26 | 0 | 2939 | 520 | 1368 | 0.0 | 0.0039 | 0 | 0 | 631910 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers | 28 | 0 | 26 | 0 | 2930 | 592 | 984 | 0.0 | 0.0039 | 0 | 0 | 633284 |
| High | reader-chat | logged-in-reader | desktop | /vi/chat | 28 | 0 | 26 | 0 | 2726 | 552 | 976 | 0.0 | 0.0039 | 0 | 0 | 631363 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet | 28 | 0 | 26 | 0 | 2951 | 592 | 1160 | 0.0 | 0.0039 | 0 | 0 | 633541 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/notifications | 28 | 0 | 26 | 0 | 2899 | 584 | 1012 | 0.0 | 0.0040 | 0 | 0 | 631748 |
| High | reading | logged-in-reader | desktop | /vi/reading/history | 28 | 0 | 26 | 0 | 3898 | 1204 | 1204 | 0.0 | 0.0039 | 0 | 0 | 632436 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/84843a3c-745f-4205-8056-030a40207dde | 28 | 0 | 26 | 0 | 2746 | 520 | 956 | 0.0 | 0.0039 | 0 | 0 | 631781 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/b27bf782-c302-4ae8-ab53-0f71b70a5c67 | 28 | 0 | 26 | 0 | 2759 | 556 | 904 | 0.0 | 0.0039 | 0 | 0 | 631616 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 2716 | 524 | 876 | 0.0 | 0.0039 | 0 | 0 | 630790 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 2772 | 548 | 1232 | 0.0 | 0.0039 | 0 | 0 | 632435 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 2808 | 576 | 1120 | 0.0 | 0.0039 | 0 | 0 | 632596 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 2742 | 560 | 1232 | 0.0 | 0.0039 | 0 | 0 | 632627 |
| High | reading | logged-in-admin | mobile | /vi/reading | 28 | 0 | 26 | 0 | 2931 | 520 | 888 | 0.0 | 0.0000 | 0 | 0 | 641107 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | 28 | 0 | 26 | 0 | 2822 | 732 | 1080 | 0.0 | 0.0760 | 0 | 0 | 630117 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers | 28 | 0 | 26 | 0 | 2847 | 476 | 1108 | 0.0 | 0.0000 | 0 | 0 | 633267 |
| High | reader-chat | logged-in-admin | mobile | /vi/chat | 28 | 0 | 26 | 0 | 2817 | 440 | 792 | 0.0 | 0.0000 | 0 | 0 | 631475 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 2813 | 448 | 796 | 0.0 | 0.0000 | 0 | 0 | 631176 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 2786 | 496 | 844 | 0.0 | 0.0000 | 0 | 0 | 631887 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 28 | 0 | 26 | 0 | 2804 | 492 | 1152 | 0.0 | 0.0071 | 0 | 0 | 630591 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/notifications | 28 | 0 | 26 | 0 | 2755 | 476 | 820 | 0.0 | 0.0000 | 0 | 0 | 631626 |
| High | reader-chat | logged-in-admin | mobile | /vi/reader/apply | 28 | 0 | 26 | 0 | 3401 | 512 | 864 | 0.0 | 0.0071 | 0 | 0 | 632017 |
| High | reading | logged-in-admin | mobile | /vi/reading/history | 28 | 0 | 26 | 0 | 3587 | 656 | 1016 | 0.0 | 0.0000 | 0 | 0 | 632278 |
| High | admin | logged-in-admin | mobile | /vi/admin/promotions | 28 | 0 | 26 | 0 | 2828 | 484 | 804 | 0.0 | 0.0000 | 0 | 0 | 644094 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 2757 | 476 | 812 | 0.0 | 0.0000 | 0 | 0 | 630531 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 2933 | 536 | 876 | 0.0 | 0.0000 | 0 | 0 | 630671 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 2750 | 452 | 804 | 0.0 | 0.0000 | 0 | 0 | 632104 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/mfa | 28 | 0 | 26 | 0 | 2843 | 456 | 828 | 0.0 | 0.0000 | 0 | 0 | 631184 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/reader | 28 | 0 | 26 | 0 | 2952 | 572 | 912 | 0.0 | 0.0000 | 0 | 0 | 632016 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers | 28 | 0 | 26 | 0 | 2903 | 516 | 872 | 0.0 | 0.0000 | 0 | 0 | 633587 |
| High | reader-chat | logged-in-reader | mobile | /vi/chat | 28 | 0 | 26 | 0 | 2938 | 516 | 896 | 0.0 | 0.0000 | 0 | 0 | 631256 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 3112 | 476 | 868 | 0.0 | 0.0000 | 0 | 0 | 631095 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 4079 | 832 | 832 | 0.0 | 0.0071 | 0 | 0 | 631471 |
| High | reader-chat | logged-in-reader | mobile | /vi/reader/apply | 28 | 0 | 26 | 0 | 4095 | 876 | 876 | 0.0 | 0.0071 | 0 | 0 | 631773 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 2885 | 480 | 832 | 0.0 | 0.0000 | 0 | 0 | 630705 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 2853 | 472 | 816 | 0.0 | 0.0000 | 0 | 0 | 630865 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 2836 | 496 | 844 | 0.0 | 0.0000 | 0 | 0 | 630798 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 2843 | 540 | 912 | 0.0 | 0.0000 | 0 | 0 | 632500 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 2819 | 472 | 824 | 0.0 | 0.0000 | 0 | 0 | 632435 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 2847 | 464 | 832 | 0.0 | 0.0000 | 0 | 0 | 632826 |
| High | auth-public | logged-in-admin | mobile | /vi/legal/tos | 27 | 1 | 23 | 0 | 3655 | 624 | 940 | 0.0 | 0.0032 | 0 | 0 | 527310 |
| High | auth-public | logged-in-admin | desktop | /vi | 26 | 0 | 24 | 0 | 2743 | 712 | 1384 | 333.0 | 0.0039 | 0 | 0 | 536864 |
| High | auth-public | logged-in-reader | desktop | /vi | 26 | 0 | 24 | 0 | 2759 | 556 | 1196 | 275.0 | 0.0033 | 0 | 0 | 537014 |
| High | auth-public | logged-in-admin | mobile | /vi | 26 | 0 | 24 | 0 | 2752 | 532 | 904 | 0.0 | 0.0032 | 0 | 0 | 537041 |
| High | auth-public | logged-in-admin | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 3708 | 688 | 1012 | 0.0 | 0.0032 | 0 | 0 | 525843 |
| High | auth-public | logged-in-reader | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 3627 | 1184 | 1512 | 0.0 | 0.0032 | 0 | 0 | 525694 |
| High | auth-public | logged-in-reader | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 3819 | 1284 | 1284 | 0.0 | 0.0032 | 0 | 0 | 525528 |
| High | auth-public | logged-out | mobile | /vi/verify-email | 24 | 0 | 22 | 0 | 2909 | 684 | 684 | 0.0 | 0.0000 | 0 | 0 | 511499 |
| Medium | auth-public | logged-out | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 2697 | 572 | 896 | 0.0 | 0.0000 | 0 | 0 | 525275 |
| Medium | auth-public | logged-out | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2702 | 476 | 804 | 0.0 | 0.0000 | 0 | 0 | 525288 |
| Medium | auth-public | logged-out | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2744 | 608 | 608 | 0.0 | 0.0000 | 0 | 0 | 525284 |
| Medium | auth-public | logged-in-admin | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 2685 | 536 | 844 | 0.0 | 0.0020 | 0 | 0 | 525600 |
| Medium | auth-public | logged-in-admin | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2698 | 532 | 936 | 0.0 | 0.0020 | 0 | 0 | 525613 |
| Medium | auth-public | logged-in-admin | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2689 | 544 | 856 | 0.0 | 0.0020 | 0 | 0 | 525659 |
| Medium | auth-public | logged-in-reader | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 3425 | 744 | 1056 | 0.0 | 0.0019 | 0 | 0 | 525539 |
| Medium | auth-public | logged-in-reader | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2970 | 668 | 1152 | 0.0 | 0.0019 | 0 | 0 | 525633 |
| Medium | auth-public | logged-in-reader | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2756 | 572 | 960 | 0.0 | 0.0019 | 0 | 0 | 525624 |
| Medium | auth-public | logged-out | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 2725 | 544 | 872 | 0.0 | 0.0000 | 0 | 0 | 525177 |
| Medium | auth-public | logged-out | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2814 | 604 | 908 | 0.0 | 0.0000 | 0 | 0 | 525244 |
| Medium | auth-public | logged-out | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2769 | 600 | 932 | 0.0 | 0.0000 | 0 | 0 | 525408 |
| Medium | auth-public | logged-in-admin | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 3305 | 720 | 720 | 0.0 | 0.0032 | 0 | 0 | 525506 |
| Medium | auth-public | logged-in-reader | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 3022 | 824 | 1172 | 0.0 | 0.0032 | 0 | 0 | 525771 |
| Medium | auth-public | logged-out | desktop | /vi/login | 24 | 0 | 22 | 0 | 2754 | 668 | 668 | 0.0 | 0.0000 | 0 | 0 | 511653 |
| Medium | auth-public | logged-out | desktop | /vi/register | 24 | 0 | 22 | 0 | 2724 | 580 | 580 | 0.0 | 0.0000 | 0 | 0 | 512196 |
| Medium | auth-public | logged-out | desktop | /vi/forgot-password | 24 | 0 | 22 | 0 | 2711 | 500 | 500 | 0.0 | 0.0000 | 0 | 0 | 511273 |
| Medium | auth-public | logged-out | desktop | /vi/reset-password | 24 | 0 | 22 | 0 | 2725 | 552 | 552 | 0.0 | 0.0000 | 0 | 0 | 511410 |
| Medium | auth-public | logged-out | desktop | /vi/verify-email | 24 | 0 | 22 | 0 | 2858 | 732 | 732 | 0.0 | 0.0000 | 0 | 0 | 511531 |
| Medium | auth-public | logged-out | mobile | /vi/login | 24 | 0 | 22 | 0 | 2691 | 500 | 500 | 0.0 | 0.0000 | 0 | 0 | 511760 |
| Medium | auth-public | logged-out | mobile | /vi/register | 24 | 0 | 22 | 0 | 2785 | 596 | 596 | 0.0 | 0.0000 | 0 | 0 | 512168 |
| Medium | auth-public | logged-out | mobile | /vi/forgot-password | 24 | 0 | 22 | 0 | 2692 | 472 | 472 | 0.0 | 0.0000 | 0 | 0 | 511351 |
| Medium | auth-public | logged-out | mobile | /vi/reset-password | 24 | 0 | 22 | 0 | 2703 | 468 | 468 | 0.0 | 0.0000 | 0 | 0 | 511371 |

## Major Issues Found

- Critical: chưa phát hiện page Critical theo benchmark hiện tại.
- High: 147 page(s) vượt ngưỡng >25 requests hoặc có request >800ms.
- Medium: 23 page(s) có request trong dải 400-800ms.
- Duplicate: chưa phát hiện duplicate request business đáng kể.
- Pending: chưa phát hiện pending request bất thường.

### Slow Requests

| Severity | Feature | Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/withdraw | GET | 200 | 1552 | 329 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| Critical | reading | logged-in-reader | mobile | /vi/reading/history | GET | 200 | 1383 | 67 | static | https://www.tarotnow.xyz/_next/static/chunks/0kn_-kiqq.6et.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/history | GET | 200 | 1380 | 85 | static | https://www.tarotnow.xyz/_next/static/chunks/0ryoba0b~me9m.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/history | GET | 200 | 1360 | 85 | static | https://www.tarotnow.xyz/_next/static/chunks/0c4xf8mjx0gok.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/history | GET | 200 | 1324 | 103 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1261 | 214 | third-party | https://img.tarotnow.xyz/light-god-50/08_The_Chariot_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1256 | 269 | third-party | https://img.tarotnow.xyz/light-god-50/08_The_Chariot_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit/history | GET | 200 | 1243 | 356 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| Critical | reading | logged-in-reader | desktop | /vi/reading/history | GET | 200 | 1233 | 421 | html | https://www.tarotnow.xyz/vi/reading/history |
| Critical | reading | logged-in-reader | mobile | /vi/reading/history | GET | 200 | 1226 | 323 | html | https://www.tarotnow.xyz/vi/reading/history |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1221 | 248 | third-party | https://img.tarotnow.xyz/light-god-50/07_The_Lovers_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1210 | 199 | third-party | https://img.tarotnow.xyz/light-god-50/07_The_Lovers_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | auth-public | logged-out | desktop | /vi | GET | 200 | 1193 | 582 | html | https://www.tarotnow.xyz/vi |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/notifications | GET | 200 | 1188 | 308 | html | https://www.tarotnow.xyz/vi/notifications |
| Critical | reading | logged-in-reader | mobile | /vi/reading/history | GET | 200 | 1152 | 68 | static | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/history | GET | 200 | 1148 | 102 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/history | GET | 200 | 1147 | 103 | static | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/legal/privacy | GET | 200 | 1146 | 360 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1142 | 276 | third-party | https://img.tarotnow.xyz/light-god-50/06_The_Hierophant_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | reading | logged-in-reader | mobile | /vi/reading/history | GET | 200 | 1130 | 193 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/history | GET | 200 | 1128 | 175 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/history | GET | 200 | 1126 | 85 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/history | GET | 200 | 1126 | 166 | static | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/legal/tos | GET | 200 | 1123 | 311 | html | https://www.tarotnow.xyz/vi/legal/tos |
| Critical | reading | logged-in-reader | mobile | /vi/reading/history | GET | 200 | 1121 | 192 | static | https://www.tarotnow.xyz/_next/static/chunks/0xovby2vf96zv.js |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit/history | GET | 200 | 1111 | 95 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/history | GET | 200 | 1105 | 130 | static | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/history | GET | 200 | 1094 | 175 | static | https://www.tarotnow.xyz/_next/static/chunks/0h38g3weqde1h.js |
| Critical | auth-public | logged-out | mobile | /vi | GET | 200 | 1087 | 557 | html | https://www.tarotnow.xyz/vi |
| Critical | reading | logged-in-reader | mobile | /vi/reading/history | GET | 200 | 1083 | 104 | static | https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/history | GET | 200 | 1076 | 82 | static | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| Critical | reader-chat | logged-in-reader | mobile | /vi/reader/apply | GET | 200 | 1070 | 102 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/history | GET | 200 | 1068 | 73 | static | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| Critical | reading | logged-in-reader | mobile | /vi/reading/history | GET | 200 | 1063 | 192 | static | https://www.tarotnow.xyz/_next/static/chunks/0d2b1s5.nlpim.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/history | GET | 200 | 1047 | 81 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| Critical | reader-chat | logged-in-reader | mobile | /vi/reader/apply | GET | 200 | 1043 | 310 | html | https://www.tarotnow.xyz/vi/reader/apply |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1039 | 148 | third-party | https://img.tarotnow.xyz/light-god-50/06_The_Hierophant_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit/history | GET | 200 | 1039 | 113 | static | https://www.tarotnow.xyz/_next/static/chunks/0h38g3weqde1h.js |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit/history | GET | 200 | 1036 | 105 | static | https://www.tarotnow.xyz/_next/static/chunks/0xovby2vf96zv.js |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit/history | GET | 200 | 1034 | 104 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| Critical | reader-chat | logged-in-reader | mobile | /vi/reader/apply | GET | 200 | 1031 | 78 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/history | GET | 200 | 1028 | 91 | static | https://www.tarotnow.xyz/_next/static/chunks/0eqce2yjfryre.js |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit/history | GET | 200 | 1025 | 106 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/history | GET | 200 | 1022 | 68 | static | https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit/history | GET | 200 | 1020 | 92 | static | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/history | GET | 200 | 1017 | 101 | static | https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/legal/tos | GET | 200 | 1011 | 333 | html | https://www.tarotnow.xyz/vi/legal/tos |
| Critical | reading | logged-in-reader | desktop | /vi/reading/history | GET | 200 | 1005 | 98 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/history | GET | 200 | 1005 | 66 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit/history | GET | 200 | 992 | 92 | static | https://www.tarotnow.xyz/_next/static/chunks/0d2b1s5.nlpim.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/history | GET | 200 | 985 | 192 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/history | GET | 200 | 979 | 105 | telemetry | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | reading | logged-in-reader | desktop | /vi/reading/history | GET | 200 | 973 | 98 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| Critical | reading | logged-in-reader | desktop | /vi/reading/history | GET | 200 | 968 | 166 | static | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 966 | 584 | third-party | https://img.tarotnow.xyz/light-god-50/05_The_Emperor_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/withdraw | GET | 200 | 961 | 201 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| Critical | reading | logged-in-reader | desktop | /vi/reading/history | GET | 200 | 959 | 146 | static | https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | GET | 200 | 957 | 309 | html | https://www.tarotnow.xyz/vi/profile |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/withdraw | GET | 200 | 944 | 124 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 931 | 148 | third-party | https://img.tarotnow.xyz/light-god-50/05_The_Emperor_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/withdraw | GET | 200 | 931 | 201 | static | https://www.tarotnow.xyz/_next/static/chunks/0kn_-kiqq.6et.js |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit/history | GET | 200 | 930 | 378 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/withdraw | GET | 200 | 928 | 145 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 918 | 149 | third-party | https://img.tarotnow.xyz/light-god-50/04_The_Empress_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | auth-public | logged-in-reader | mobile | /vi/legal/privacy | GET | 200 | 914 | 174 | static | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| Critical | reading | logged-in-reader | desktop | /vi/reading/history | GET | 200 | 910 | 165 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/history | GET | 200 | 908 | 175 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | reader-chat | logged-in-reader | mobile | /vi/reader/apply | GET | 200 | 899 | 138 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/history | GET | 200 | 896 | 91 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | reading | logged-in-reader | desktop | /vi/reading/history | GET | 200 | 892 | 101 | static | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| Critical | reading | logged-in-reader | desktop | /vi/reading/history | GET | 200 | 887 | 74 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit/history | GET | 200 | 886 | 66 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | GET | 200 | 883 | 393 | html | https://www.tarotnow.xyz/vi/inventory |
| Critical | auth-public | logged-in-reader | mobile | /vi/legal/privacy | GET | 200 | 881 | 179 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/withdraw | GET | 200 | 879 | 202 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| Critical | reader-chat | logged-in-reader | mobile | /vi/reader/apply | GET | 200 | 874 | 156 | static | https://www.tarotnow.xyz/_next/static/chunks/0kn_-kiqq.6et.js |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 873 | 239 | third-party | https://img.tarotnow.xyz/light-god-50/04_The_Empress_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/inventory | GET | 200 | 872 | 329 | html | https://www.tarotnow.xyz/vi/inventory |
| Critical | reading | logged-in-reader | desktop | /vi/reading/history | GET | 200 | 862 | 107 | static | https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| Critical | reading | logged-in-reader | desktop | /vi/reading/history | GET | 200 | 862 | 103 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |

### Duplicate API / Request Candidates

| Severity | Feature | Scenario | Viewport | Route | Count | Request Key |
| --- | --- | --- | --- | --- | ---: | --- |
| - | - | - | - | - | - | - |

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
