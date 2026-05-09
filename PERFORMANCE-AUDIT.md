# PERFORMANCE AUDIT - TarotNow

## Executive Summary

- Generated at (UTC): 2026-05-09T01:03:40.083Z
- Benchmark generated at (UTC): 2026-05-09T01:03:31.379Z
- Benchmark input: Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark.json
- Base origin: https://www.tarotnow.xyz
- Locale prefix: /vi
- Benchmark mode: full-matrix
- Total scenarios: 6
- Total pages measured: 170
- Critical pages: 0
- High pages: 143
- Medium pages: 27
- Slow requests >800ms: 79
- Slow requests 400-800ms: 245
- Request thresholds: >25 suspicious, >35 severe
- Slow request thresholds: >400ms optimize, >800ms serious

## Scenario Coverage

| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Failed | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 24.9 | 2952 | 0 | 0 | yes |
| logged-in-admin | desktop | 43 | 29.0 | 3050 | 0 | 0 | yes |
| logged-in-reader | desktop | 33 | 28.7 | 3011 | 0 | 0 | yes |
| logged-out | mobile | 9 | 25.0 | 2797 | 0 | 0 | yes |
| logged-in-admin | mobile | 43 | 28.7 | 2949 | 0 | 0 | yes |
| logged-in-reader | mobile | 33 | 29.2 | 2955 | 0 | 0 | yes |

## Route Family Coverage

| Scenario | Viewport | Family | Pages | Avg requests/page | Avg nav (ms) | Pending | Failed |
| --- | --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.4 | 2926 | 0 | 0 |
| logged-in-admin | desktop | auth-public | 3 | 25.0 | 2723 | 0 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | 3 | 32.0 | 3590 | 0 | 0 |
| logged-in-admin | desktop | home | 1 | 26.0 | 2827 | 0 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | 4 | 30.8 | 4208 | 0 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | 8 | 28.5 | 2934 | 0 | 0 |
| logged-in-admin | desktop | reader-chat | 9 | 28.1 | 2802 | 0 | 0 |
| logged-in-admin | desktop | reading | 5 | 30.2 | 2918 | 0 | 0 |
| logged-in-admin | mobile | admin | 10 | 29.2 | 2788 | 0 | 0 |
| logged-in-admin | mobile | auth-public | 3 | 25.0 | 2787 | 0 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | 3 | 31.3 | 3179 | 0 | 0 |
| logged-in-admin | mobile | home | 1 | 26.0 | 2786 | 0 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | 4 | 30.5 | 3573 | 0 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | 8 | 28.4 | 2857 | 0 | 0 |
| logged-in-admin | mobile | reader-chat | 9 | 28.2 | 2845 | 0 | 0 |
| logged-in-admin | mobile | reading | 5 | 29.2 | 3098 | 0 | 0 |
| logged-in-reader | desktop | auth-public | 3 | 25.0 | 2744 | 0 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | 3 | 31.7 | 3149 | 0 | 0 |
| logged-in-reader | desktop | home | 1 | 26.0 | 2849 | 0 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | 4 | 30.0 | 3941 | 0 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | 8 | 28.6 | 2874 | 0 | 0 |
| logged-in-reader | desktop | reader-chat | 9 | 28.2 | 2847 | 0 | 0 |
| logged-in-reader | desktop | reading | 5 | 29.8 | 2890 | 0 | 0 |
| logged-in-reader | mobile | auth-public | 3 | 25.0 | 2737 | 0 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | 3 | 31.7 | 3145 | 0 | 0 |
| logged-in-reader | mobile | home | 1 | 26.0 | 2724 | 0 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | 4 | 31.3 | 3561 | 0 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | 8 | 28.9 | 2878 | 0 | 0 |
| logged-in-reader | mobile | reader-chat | 9 | 28.3 | 2803 | 0 | 0 |
| logged-in-reader | mobile | reading | 5 | 31.2 | 2928 | 0 | 0 |
| logged-out | desktop | auth-public | 8 | 24.4 | 2801 | 0 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 4164 | 0 | 0 |
| logged-out | mobile | auth-public | 8 | 24.5 | 2742 | 0 | 0 |
| logged-out | mobile | home | 1 | 29.0 | 3240 | 0 | 0 |

## Detailed Metrics Table

| Severity | Feature | Scenario | Viewport | Route | Requests | API | Static | Third-party | Nav (ms) | FCP (ms) | LCP (ms) | TBT (ms) | CLS | Pending | Failed | Transfer bytes |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/community | 35 | 1 | 31 | 1 | 4079 | 560 | 1184 | 0.0 | 0.0041 | 0 | 0 | 794729 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/community | 35 | 1 | 31 | 1 | 3707 | 532 | 1172 | 0.0 | 0.0037 | 0 | 0 | 794399 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/community | 35 | 1 | 31 | 1 | 3703 | 504 | 1020 | 0.0 | 0.0000 | 0 | 0 | 794461 |
| High | community-leaderboard-quest | logged-in-reader | mobile | /vi/community | 35 | 1 | 31 | 1 | 3638 | 448 | 1000 | 0.0 | 0.0051 | 0 | 0 | 794758 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/1f3e1624-3f42-4a7e-be51-7a1e0a6614fd | 34 | 2 | 30 | 0 | 3052 | 548 | 1020 | 0.0 | 0.0041 | 0 | 0 | 724467 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/f6282261-e10f-4164-88d2-fc61ba2d07a6 | 34 | 3 | 29 | 0 | 3008 | 496 | 868 | 0.0 | 0.0000 | 0 | 0 | 694003 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/016e08da-ef18-4d00-a6b7-b6da8d4be140 | 33 | 2 | 29 | 0 | 2900 | 512 | 924 | 0.0 | 0.0054 | 0 | 0 | 714067 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/276d9278-e955-4065-a0ae-f91eddf72cfa | 33 | 2 | 29 | 0 | 2967 | 460 | 836 | 0.0 | 0.0000 | 0 | 0 | 692590 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/91d9c17c-18e9-4aa6-a25c-0fa3ccfb7687 | 33 | 2 | 29 | 0 | 3005 | 468 | 836 | 0.0 | 0.0000 | 0 | 0 | 693080 |
| High | admin | logged-in-admin | desktop | /vi/admin/gamification | 32 | 0 | 30 | 0 | 3298 | 576 | 948 | 0.0 | 0.0022 | 0 | 0 | 697925 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/439f7c39-42cb-4656-8510-7e072b517d33 | 32 | 1 | 29 | 0 | 2971 | 592 | 996 | 0.0 | 0.0037 | 0 | 0 | 713030 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/55609b45-fcff-40a7-9984-deea43579c42 | 32 | 1 | 29 | 0 | 2935 | 588 | 1004 | 0.0 | 0.0037 | 0 | 0 | 713153 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha | 32 | 2 | 28 | 0 | 2857 | 440 | 1088 | 0.0 | 0.0000 | 0 | 0 | 726283 |
| High | admin | logged-in-admin | mobile | /vi/admin/gamification | 32 | 0 | 30 | 0 | 2878 | 472 | 812 | 0.0 | 0.0000 | 0 | 0 | 698011 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/inventory | 32 | 2 | 28 | 0 | 2881 | 436 | 1092 | 0.0 | 0.0000 | 0 | 0 | 645726 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha | 32 | 2 | 28 | 0 | 2912 | 440 | 1112 | 0.0 | 0.0000 | 0 | 0 | 726331 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha/history | 32 | 2 | 28 | 0 | 2880 | 432 | 1120 | 0.0 | 0.0000 | 0 | 0 | 725521 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | 31 | 1 | 28 | 0 | 2900 | 560 | 1224 | 0.0 | 0.0041 | 0 | 0 | 645089 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha | 31 | 1 | 28 | 0 | 2909 | 576 | 1264 | 0.0 | 0.0041 | 0 | 0 | 725333 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha/history | 31 | 1 | 28 | 0 | 2886 | 560 | 1448 | 0.0 | 0.0041 | 0 | 0 | 726546 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile | 31 | 2 | 27 | 0 | 3446 | 584 | 1516 | 0.0 | 0.0489 | 0 | 0 | 635357 |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/leaderboard | 31 | 1 | 28 | 0 | 3335 | 728 | 1144 | 0.0 | 0.0041 | 0 | 0 | 650789 |
| High | admin | logged-in-admin | desktop | /vi/admin/disputes | 31 | 1 | 28 | 0 | 2960 | 596 | 908 | 0.0 | 0.0000 | 0 | 0 | 648351 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha | 31 | 1 | 28 | 0 | 2898 | 572 | 1280 | 0.0 | 0.0037 | 0 | 0 | 725679 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/inventory | 31 | 1 | 28 | 0 | 2858 | 444 | 1100 | 0.0 | 0.0000 | 0 | 0 | 644696 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/d29e47ea-d1c9-44ca-ab5d-de36c5485b1b | 31 | 1 | 28 | 0 | 2861 | 428 | 784 | 0.0 | 0.0000 | 0 | 0 | 681214 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | 30 | 1 | 27 | 0 | 8138 | 604 | 1048 | 56.0 | 0.0574 | 0 | 0 | 643625 |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | 30 | 1 | 27 | 0 | 3356 | 608 | 1012 | 0.0 | 0.0279 | 0 | 0 | 643065 |
| High | admin | logged-in-admin | desktop | /vi/admin/users | 30 | 0 | 28 | 0 | 2887 | 632 | 1320 | 0.0 | 0.0000 | 0 | 0 | 649359 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/inventory | 30 | 0 | 28 | 0 | 2910 | 580 | 1268 | 0.0 | 0.0037 | 0 | 0 | 644071 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha/history | 30 | 0 | 28 | 0 | 2883 | 536 | 1356 | 0.0 | 0.0037 | 0 | 0 | 723575 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile | 30 | 1 | 27 | 0 | 3091 | 648 | 1252 | 0.0 | 0.0723 | 0 | 0 | 634534 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers | 30 | 2 | 26 | 0 | 2945 | 536 | 1204 | 0.0 | 0.0037 | 0 | 0 | 635251 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/leaderboard | 30 | 0 | 28 | 0 | 2844 | 528 | 968 | 0.0 | 0.0037 | 0 | 0 | 650141 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/gamification | 30 | 1 | 27 | 0 | 2895 | 532 | 1016 | 0.0 | 0.0274 | 0 | 0 | 643159 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/notifications | 30 | 2 | 26 | 0 | 2920 | 540 | 964 | 0.0 | 0.0037 | 0 | 0 | 633405 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha/history | 30 | 0 | 28 | 0 | 2849 | 432 | 840 | 0.0 | 0.0000 | 0 | 0 | 725695 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile | 30 | 1 | 27 | 0 | 3135 | 524 | 884 | 0.0 | 0.0000 | 0 | 0 | 634328 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/leaderboard | 30 | 0 | 28 | 0 | 2957 | 540 | 920 | 0.0 | 0.0000 | 0 | 0 | 649766 |
| High | admin | logged-in-admin | mobile | /vi/admin/users | 30 | 0 | 28 | 0 | 2773 | 484 | 820 | 0.0 | 0.0000 | 0 | 0 | 649492 |
| High | reading | logged-in-reader | mobile | /vi/reading | 30 | 2 | 26 | 0 | 2897 | 448 | 840 | 0.0 | 0.0000 | 0 | 0 | 642894 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile | 30 | 1 | 27 | 0 | 3013 | 448 | 1088 | 0.0 | 0.0821 | 0 | 0 | 634773 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers | 30 | 2 | 26 | 0 | 2928 | 504 | 1136 | 0.0 | 0.0000 | 0 | 0 | 635233 |
| High | community-leaderboard-quest | logged-in-reader | mobile | /vi/leaderboard | 30 | 0 | 28 | 0 | 2891 | 456 | 844 | 0.0 | 0.0000 | 0 | 0 | 650334 |
| High | community-leaderboard-quest | logged-in-reader | mobile | /vi/gamification | 30 | 1 | 27 | 0 | 2906 | 480 | 884 | 0.0 | 0.0000 | 0 | 0 | 643433 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet | 30 | 2 | 26 | 0 | 2900 | 456 | 1116 | 0.0 | 0.0000 | 0 | 0 | 635457 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/notifications | 30 | 2 | 26 | 0 | 2939 | 476 | 820 | 0.0 | 0.0000 | 0 | 0 | 633561 |
| High | auth-public | logged-out | desktop | /vi | 29 | 0 | 27 | 0 | 4164 | 1192 | 1760 | 58.0 | 0.0000 | 0 | 0 | 600829 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers | 29 | 1 | 26 | 0 | 2907 | 584 | 1216 | 0.0 | 0.0000 | 0 | 0 | 633886 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet | 29 | 1 | 26 | 0 | 2963 | 592 | 1356 | 0.0 | 0.0041 | 0 | 0 | 634284 |
| High | admin | logged-in-admin | desktop | /vi/admin | 29 | 0 | 27 | 0 | 3240 | 596 | 1064 | 0.0 | 0.0000 | 0 | 0 | 647089 |
| High | admin | logged-in-admin | desktop | /vi/admin/deposits | 29 | 0 | 27 | 0 | 2939 | 676 | 1108 | 0.0 | 0.0000 | 0 | 0 | 647260 |
| High | admin | logged-in-admin | desktop | /vi/admin/reader-requests | 29 | 0 | 27 | 0 | 2805 | 568 | 908 | 0.0 | 0.0000 | 0 | 0 | 645936 |
| High | admin | logged-in-admin | desktop | /vi/admin/readings | 29 | 0 | 27 | 0 | 2857 | 564 | 1136 | 0.0 | 0.0000 | 0 | 0 | 648277 |
| High | admin | logged-in-admin | desktop | /vi/admin/withdrawals | 29 | 0 | 27 | 0 | 2719 | 552 | 892 | 0.0 | 0.0000 | 0 | 0 | 645622 |
| High | reading | logged-in-reader | desktop | /vi/reading | 29 | 1 | 26 | 0 | 2937 | 560 | 1000 | 0.0 | 0.0037 | 0 | 0 | 642458 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | 29 | 0 | 27 | 0 | 7074 | 604 | 604 | 0.0 | 0.0569 | 0 | 0 | 641784 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/withdraw | 29 | 1 | 26 | 0 | 2975 | 584 | 1228 | 0.0 | 0.0092 | 0 | 0 | 633185 |
| High | auth-public | logged-out | mobile | /vi | 29 | 0 | 27 | 0 | 3240 | 972 | 972 | 0.0 | 0.0000 | 0 | 0 | 600795 |
| High | reading | logged-in-admin | mobile | /vi/reading | 29 | 1 | 26 | 0 | 4169 | 444 | 2376 | 0.0 | 0.0071 | 0 | 0 | 642321 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | 29 | 0 | 27 | 0 | 5726 | 468 | 468 | 27.0 | 0.0000 | 0 | 0 | 642843 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers | 29 | 1 | 26 | 0 | 2949 | 460 | 916 | 0.0 | 0.0000 | 0 | 0 | 633884 |
| High | reader-chat | logged-in-admin | mobile | /vi/chat | 29 | 1 | 26 | 0 | 2949 | 552 | 920 | 0.0 | 0.0000 | 0 | 0 | 632081 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 29 | 0 | 27 | 0 | 2878 | 508 | 868 | 0.0 | 0.0000 | 0 | 0 | 642041 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet | 29 | 1 | 26 | 0 | 2872 | 448 | 1124 | 0.0 | 0.0000 | 0 | 0 | 634199 |
| High | admin | logged-in-admin | mobile | /vi/admin | 29 | 0 | 27 | 0 | 2829 | 484 | 836 | 0.0 | 0.0000 | 0 | 0 | 646863 |
| High | admin | logged-in-admin | mobile | /vi/admin/deposits | 29 | 0 | 27 | 0 | 2799 | 452 | 784 | 0.0 | 0.0000 | 0 | 0 | 647186 |
| High | admin | logged-in-admin | mobile | /vi/admin/disputes | 29 | 0 | 27 | 0 | 2740 | 436 | 756 | 0.0 | 0.0000 | 0 | 0 | 645376 |
| High | admin | logged-in-admin | mobile | /vi/admin/reader-requests | 29 | 0 | 27 | 0 | 2786 | 468 | 808 | 0.0 | 0.0000 | 0 | 0 | 645926 |
| High | admin | logged-in-admin | mobile | /vi/admin/readings | 29 | 0 | 27 | 0 | 2792 | 464 | 844 | 0.0 | 0.0000 | 0 | 0 | 648251 |
| High | admin | logged-in-admin | mobile | /vi/admin/withdrawals | 29 | 0 | 27 | 0 | 2719 | 456 | 784 | 0.0 | 0.0000 | 0 | 0 | 645710 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | 29 | 0 | 27 | 0 | 5570 | 464 | 464 | 0.0 | 0.0000 | 0 | 0 | 641835 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/withdraw | 29 | 1 | 26 | 0 | 2877 | 448 | 1104 | 0.0 | 0.0330 | 0 | 0 | 633305 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 29 | 1 | 26 | 0 | 2822 | 436 | 768 | 0.0 | 0.0000 | 0 | 0 | 631632 |
| High | reading | logged-in-admin | desktop | /vi/reading | 28 | 0 | 26 | 0 | 2974 | 604 | 1040 | 0.0 | 0.0041 | 0 | 0 | 641328 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | 28 | 0 | 26 | 0 | 2939 | 576 | 1032 | 0.0 | 0.0041 | 0 | 0 | 631247 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | 28 | 0 | 26 | 0 | 2788 | 560 | 1256 | 0.0 | 0.0489 | 0 | 0 | 630235 |
| High | reader-chat | logged-in-admin | desktop | /vi/chat | 28 | 0 | 26 | 0 | 2793 | 540 | 992 | 0.0 | 0.0041 | 0 | 0 | 631244 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 2788 | 544 | 1004 | 0.0 | 0.0041 | 0 | 0 | 630915 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 2919 | 584 | 1232 | 0.0 | 0.0041 | 0 | 0 | 631660 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | 28 | 0 | 26 | 0 | 2775 | 584 | 1320 | 0.0 | 0.0041 | 0 | 0 | 630613 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/notifications | 28 | 0 | 26 | 0 | 2856 | 648 | 1084 | 0.0 | 0.0041 | 0 | 0 | 631468 |
| High | reader-chat | logged-in-admin | desktop | /vi/reader/apply | 28 | 0 | 26 | 0 | 2717 | 556 | 968 | 0.0 | 0.0041 | 0 | 0 | 631849 |
| High | reading | logged-in-admin | desktop | /vi/reading/history | 28 | 0 | 26 | 0 | 2877 | 568 | 1272 | 0.0 | 0.0041 | 0 | 0 | 632415 |
| High | admin | logged-in-admin | desktop | /vi/admin/promotions | 28 | 0 | 26 | 0 | 2762 | 576 | 920 | 0.0 | 0.0000 | 0 | 0 | 644066 |
| High | admin | logged-in-admin | desktop | /vi/admin/system-configs | 28 | 0 | 26 | 0 | 2795 | 552 | 1172 | 0.0 | 0.0000 | 0 | 0 | 655174 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/6baaa5d9-186d-4861-b0e9-bcf41ccb11ab | 28 | 0 | 26 | 0 | 2789 | 564 | 1008 | 0.0 | 0.0041 | 0 | 0 | 631882 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 2797 | 612 | 1044 | 0.0 | 0.0041 | 0 | 0 | 630532 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 2709 | 520 | 924 | 0.0 | 0.0041 | 0 | 0 | 630545 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 2810 | 592 | 1020 | 0.0 | 0.0041 | 0 | 0 | 630531 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 2802 | 560 | 1260 | 0.0 | 0.0041 | 0 | 0 | 632252 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 2796 | 592 | 1240 | 0.0 | 0.0041 | 0 | 0 | 632455 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 2891 | 560 | 1220 | 0.0 | 0.0041 | 0 | 0 | 632526 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/mfa | 28 | 0 | 26 | 0 | 2755 | 568 | 940 | 0.0 | 0.0037 | 0 | 0 | 631284 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/reader | 28 | 0 | 26 | 0 | 2781 | 544 | 1336 | 0.0 | 0.0037 | 0 | 0 | 632025 |
| High | reader-chat | logged-in-reader | desktop | /vi/chat | 28 | 0 | 26 | 0 | 2811 | 644 | 1060 | 0.0 | 0.0037 | 0 | 0 | 631085 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet | 28 | 0 | 26 | 0 | 2896 | 596 | 1320 | 0.0 | 0.0037 | 0 | 0 | 633571 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 2784 | 536 | 956 | 0.0 | 0.0037 | 0 | 0 | 631322 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 2793 | 528 | 976 | 0.0 | 0.0037 | 0 | 0 | 631338 |
| High | reader-chat | logged-in-reader | desktop | /vi/reader/apply | 28 | 0 | 26 | 0 | 2696 | 500 | 892 | 0.0 | 0.0037 | 0 | 0 | 632079 |
| High | reading | logged-in-reader | desktop | /vi/reading/history | 28 | 0 | 26 | 0 | 2777 | 544 | 1196 | 0.0 | 0.0037 | 0 | 0 | 632449 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/916b800e-2191-4c36-9fee-7b7c0e946bd3 | 28 | 0 | 26 | 0 | 2830 | 572 | 1012 | 0.0 | 0.0037 | 0 | 0 | 631829 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 2917 | 632 | 1028 | 0.0 | 0.0037 | 0 | 0 | 630983 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 2857 | 516 | 956 | 0.0 | 0.0037 | 0 | 0 | 630839 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 2880 | 568 | 988 | 0.0 | 0.0037 | 0 | 0 | 630911 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 2733 | 552 | 1220 | 0.0 | 0.0037 | 0 | 0 | 632458 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 2913 | 564 | 1332 | 0.0 | 0.0037 | 0 | 0 | 632310 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 2867 | 560 | 1180 | 0.0 | 0.0037 | 0 | 0 | 632825 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/mfa | 28 | 0 | 26 | 0 | 2829 | 468 | 812 | 0.0 | 0.0000 | 0 | 0 | 631361 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | 28 | 0 | 26 | 0 | 2861 | 616 | 968 | 0.0 | 0.0760 | 0 | 0 | 630353 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 2759 | 456 | 804 | 0.0 | 0.0000 | 0 | 0 | 631052 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 2858 | 492 | 940 | 0.0 | 0.0000 | 0 | 0 | 631803 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 28 | 0 | 26 | 0 | 2753 | 436 | 1112 | 0.0 | 0.0071 | 0 | 0 | 630543 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/notifications | 28 | 0 | 26 | 0 | 2790 | 448 | 808 | 0.0 | 0.0000 | 0 | 0 | 631477 |
| High | reader-chat | logged-in-admin | mobile | /vi/reader/apply | 28 | 0 | 26 | 0 | 2857 | 480 | 836 | 0.0 | 0.0000 | 0 | 0 | 632143 |
| High | reading | logged-in-admin | mobile | /vi/reading/history | 28 | 0 | 26 | 0 | 2869 | 532 | 892 | 0.0 | 0.0000 | 0 | 0 | 632501 |
| High | admin | logged-in-admin | mobile | /vi/admin/promotions | 28 | 0 | 26 | 0 | 2728 | 440 | 776 | 0.0 | 0.0000 | 0 | 0 | 644243 |
| High | admin | logged-in-admin | mobile | /vi/admin/system-configs | 28 | 0 | 26 | 0 | 2838 | 572 | 972 | 0.0 | 0.0000 | 0 | 0 | 655406 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/21dc990c-0b64-49ad-8ec2-4cf7f0a15ccc | 28 | 0 | 26 | 0 | 2711 | 468 | 820 | 0.0 | 0.0000 | 0 | 0 | 631709 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/dd776bf0-5445-422b-a871-d1b8295fbea3 | 28 | 0 | 26 | 0 | 2773 | 484 | 836 | 0.0 | 0.0000 | 0 | 0 | 631738 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 2715 | 428 | 752 | 0.0 | 0.0000 | 0 | 0 | 630838 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 2856 | 480 | 832 | 0.0 | 0.0000 | 0 | 0 | 630481 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 2801 | 452 | 788 | 0.0 | 0.0000 | 0 | 0 | 630518 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 2839 | 476 | 936 | 0.0 | 0.0000 | 0 | 0 | 632420 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 2838 | 660 | 1004 | 0.0 | 0.0000 | 0 | 0 | 632506 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 2801 | 440 | 896 | 0.0 | 0.0000 | 0 | 0 | 632578 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/mfa | 28 | 0 | 26 | 0 | 2906 | 544 | 884 | 0.0 | 0.0000 | 0 | 0 | 631487 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/reader | 28 | 0 | 26 | 0 | 2725 | 444 | 796 | 0.0 | 0.0000 | 0 | 0 | 631821 |
| High | reader-chat | logged-in-reader | mobile | /vi/chat | 28 | 0 | 26 | 0 | 2798 | 440 | 788 | 0.0 | 0.0000 | 0 | 0 | 631498 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 2871 | 504 | 852 | 0.0 | 0.0000 | 0 | 0 | 631272 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 2793 | 436 | 780 | 0.0 | 0.0000 | 0 | 0 | 631481 |
| High | reader-chat | logged-in-reader | mobile | /vi/reader/apply | 28 | 0 | 26 | 0 | 2734 | 436 | 768 | 0.0 | 0.0000 | 0 | 0 | 632339 |
| High | reading | logged-in-reader | mobile | /vi/reading/history | 28 | 0 | 26 | 0 | 2867 | 496 | 848 | 0.0 | 0.0000 | 0 | 0 | 632609 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 2787 | 444 | 768 | 0.0 | 0.0000 | 0 | 0 | 630927 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 2866 | 480 | 836 | 0.0 | 0.0000 | 0 | 0 | 630865 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 2746 | 480 | 832 | 0.0 | 0.0000 | 0 | 0 | 632464 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 2806 | 432 | 800 | 0.0 | 0.0000 | 0 | 0 | 632562 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 2739 | 468 | 816 | 0.0 | 0.0000 | 0 | 0 | 632717 |
| High | auth-public | logged-in-admin | desktop | /vi | 26 | 0 | 24 | 0 | 2827 | 592 | 1324 | 431.0 | 0.0035 | 0 | 0 | 537094 |
| High | auth-public | logged-in-reader | desktop | /vi | 26 | 0 | 24 | 0 | 2849 | 684 | 1372 | 375.0 | 0.0033 | 0 | 0 | 537153 |
| High | auth-public | logged-in-admin | mobile | /vi | 26 | 0 | 24 | 0 | 2786 | 532 | 912 | 0.0 | 0.0032 | 0 | 0 | 536951 |
| High | auth-public | logged-in-reader | mobile | /vi | 26 | 0 | 24 | 0 | 2724 | 460 | 836 | 0.0 | 0.0028 | 0 | 0 | 537043 |
| High | auth-public | logged-out | desktop | /vi/verify-email | 24 | 0 | 22 | 0 | 3046 | 864 | 864 | 0.0 | 0.0000 | 0 | 0 | 511564 |
| Medium | auth-public | logged-out | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 2672 | 520 | 520 | 0.0 | 0.0000 | 0 | 0 | 525180 |
| Medium | auth-public | logged-out | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2834 | 476 | 828 | 0.0 | 0.0000 | 0 | 0 | 525411 |
| Medium | auth-public | logged-out | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2732 | 500 | 500 | 0.0 | 0.0000 | 0 | 0 | 525392 |
| Medium | auth-public | logged-in-admin | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 2716 | 560 | 924 | 0.0 | 0.0020 | 0 | 0 | 525553 |
| Medium | auth-public | logged-in-admin | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2730 | 536 | 900 | 0.0 | 0.0020 | 0 | 0 | 525643 |
| Medium | auth-public | logged-in-admin | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2724 | 556 | 920 | 0.0 | 0.0020 | 0 | 0 | 525751 |
| Medium | auth-public | logged-in-reader | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 2819 | 612 | 948 | 0.0 | 0.0018 | 0 | 0 | 525674 |
| Medium | auth-public | logged-in-reader | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2691 | 580 | 864 | 0.0 | 0.0018 | 0 | 0 | 525606 |
| Medium | auth-public | logged-in-reader | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2723 | 564 | 908 | 0.0 | 0.0018 | 0 | 0 | 525848 |
| Medium | auth-public | logged-out | mobile | /vi/register | 25 | 1 | 22 | 0 | 2760 | 628 | 628 | 0.0 | 0.0000 | 0 | 0 | 512862 |
| Medium | auth-public | logged-out | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 2731 | 488 | 820 | 0.0 | 0.0000 | 0 | 0 | 525301 |
| Medium | auth-public | logged-out | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2777 | 472 | 804 | 0.0 | 0.0000 | 0 | 0 | 525298 |
| Medium | auth-public | logged-out | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2753 | 484 | 816 | 0.0 | 0.0000 | 0 | 0 | 525379 |
| Medium | auth-public | logged-in-admin | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 2787 | 500 | 816 | 0.0 | 0.0032 | 0 | 0 | 525655 |
| Medium | auth-public | logged-in-admin | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2861 | 728 | 1060 | 0.0 | 0.0032 | 0 | 0 | 525672 |
| Medium | auth-public | logged-in-admin | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2713 | 448 | 784 | 0.0 | 0.0032 | 0 | 0 | 525774 |
| Medium | auth-public | logged-in-reader | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 2707 | 432 | 748 | 0.0 | 0.0028 | 0 | 0 | 525717 |
| Medium | auth-public | logged-in-reader | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2809 | 536 | 872 | 0.0 | 0.0028 | 0 | 0 | 525749 |
| Medium | auth-public | logged-in-reader | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2695 | 436 | 768 | 0.0 | 0.0028 | 0 | 0 | 525756 |
| Medium | auth-public | logged-out | desktop | /vi/login | 24 | 0 | 22 | 0 | 2827 | 740 | 740 | 0.0 | 0.0000 | 0 | 0 | 511749 |
| Medium | auth-public | logged-out | desktop | /vi/register | 24 | 0 | 22 | 0 | 2751 | 540 | 540 | 0.0 | 0.0000 | 0 | 0 | 512302 |
| Medium | auth-public | logged-out | desktop | /vi/forgot-password | 24 | 0 | 22 | 0 | 2770 | 496 | 496 | 0.0 | 0.0000 | 0 | 0 | 511315 |
| Medium | auth-public | logged-out | desktop | /vi/reset-password | 24 | 0 | 22 | 0 | 2774 | 596 | 596 | 0.0 | 0.0000 | 0 | 0 | 511501 |
| Medium | auth-public | logged-out | mobile | /vi/login | 24 | 0 | 22 | 0 | 2739 | 612 | 612 | 0.0 | 0.0000 | 0 | 0 | 511693 |
| Medium | auth-public | logged-out | mobile | /vi/forgot-password | 24 | 0 | 22 | 0 | 2681 | 472 | 472 | 0.0 | 0.0000 | 0 | 0 | 511326 |
| Medium | auth-public | logged-out | mobile | /vi/reset-password | 24 | 0 | 22 | 0 | 2699 | 476 | 476 | 0.0 | 0.0000 | 0 | 0 | 511444 |
| Medium | auth-public | logged-out | mobile | /vi/verify-email | 24 | 0 | 22 | 0 | 2796 | 604 | 604 | 0.0 | 0.0000 | 0 | 0 | 511570 |

## Major Issues Found

- Critical: chưa phát hiện page Critical theo benchmark hiện tại.
- High: 143 page(s) vượt ngưỡng >25 requests hoặc có request >800ms.
- Medium: 27 page(s) có request trong dải 400-800ms.
- Duplicate: chưa phát hiện duplicate request business đáng kể.
- Pending: chưa phát hiện pending request bất thường.

### Slow Requests

| Severity | Feature | Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 3848 | 733 | third-party | https://img.tarotnow.xyz/light-god-50/74_Ten_of_Wands_50_20260325_181435.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 3614 | 360 | third-party | https://img.tarotnow.xyz/light-god-50/73_Nine_of_Wands_50_20260325_181435.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 3596 | 711 | third-party | https://img.tarotnow.xyz/light-god-50/71_Seven_of_Wands_50_20260325_181433.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 3575 | 571 | third-party | https://img.tarotnow.xyz/light-god-50/72_Eight_of_Wands_50_20260325_181433.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 3146 | 416 | third-party | https://img.tarotnow.xyz/light-god-50/70_Six_of_Wands_50_20260325_181433.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 3134 | 199 | third-party | https://img.tarotnow.xyz/light-god-50/69_Five_of%20_Wands_50_20260325_181431.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 3017 | 183 | third-party | https://img.tarotnow.xyz/light-god-50/68_Four_of_Wands_50_20260325_181431.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 2902 | 236 | third-party | https://img.tarotnow.xyz/light-god-50/67_Three_of_Wands_50_20260325_181431.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 2822 | 325 | third-party | https://img.tarotnow.xyz/light-god-50/66_Two_of_Wands_50_20260325_181428.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 2759 | 733 | third-party | https://img.tarotnow.xyz/light-god-50/22_The_World_50_20260325_181401.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 2291 | 231 | third-party | https://img.tarotnow.xyz/light-god-50/65_Ace_of_Wands_50_20260325_181428.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 2233 | 436 | third-party | https://img.tarotnow.xyz/light-god-50/20_The_Sun_50_20260325_181359.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 2177 | 673 | third-party | https://img.tarotnow.xyz/light-god-50/18_The_Star_50_20260325_181357.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | reading | logged-in-admin | mobile | /vi/reading | GET | 200 | 2095 | 314 | html | https://www.tarotnow.xyz/vi/reading |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 2087 | 314 | third-party | https://img.tarotnow.xyz/light-god-50/21_Judgement_50_20260325_181359.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1974 | 407 | third-party | https://img.tarotnow.xyz/light-god-50/17_The_Tower_50_20260325_181357.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1970 | 196 | third-party | https://img.tarotnow.xyz/light-god-50/19_The_Moon_50_20260325_181359.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1969 | 162 | third-party | https://img.tarotnow.xyz/light-god-50/15_Temperance_50_20260325_181356.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1968 | 190 | third-party | https://img.tarotnow.xyz/light-god-50/16_The_Devil_50_20260325_181357.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1892 | 326 | third-party | https://img.tarotnow.xyz/light-god-50/14_Death_50_20260325_181356.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1850 | 149 | third-party | https://img.tarotnow.xyz/light-god-50/13_The_Hanged_Man_50_20260325_181356.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1735 | 139 | third-party | https://img.tarotnow.xyz/light-god-50/12_Justice_50_20260325_181353.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1651 | 248 | third-party | https://img.tarotnow.xyz/light-god-50/11_Wheel_of%20_Fortune_50_20260325_181353.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1462 | 247 | third-party | https://img.tarotnow.xyz/light-god-50/10_The_Hermit_50_20260325_181353.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1438 | 304 | third-party | https://img.tarotnow.xyz/light-god-50/09_Strength_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile | GET | 200 | 1264 | 351 | html | https://www.tarotnow.xyz/vi/profile |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1249 | 158 | third-party | https://img.tarotnow.xyz/light-god-50/16_The_Devil_50_20260325_181357.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1238 | 314 | third-party | https://img.tarotnow.xyz/light-god-50/08_The_Chariot_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1237 | 272 | third-party | https://img.tarotnow.xyz/light-god-50/15_Temperance_50_20260325_181356.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1220 | 123 | third-party | https://img.tarotnow.xyz/light-god-50/05_The_Emperor_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1217 | 258 | third-party | https://img.tarotnow.xyz/light-god-50/07_The_Lovers_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1192 | 269 | third-party | https://img.tarotnow.xyz/light-god-50/14_Death_50_20260325_181356.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1179 | 259 | third-party | https://img.tarotnow.xyz/light-god-50/22_The_World_50_20260325_181401.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | auth-public | logged-out | desktop | /vi | GET | 200 | 1160 | 550 | static | https://www.tarotnow.xyz/_next/static/chunks/0gg0893b49orw.js |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1121 | 234 | third-party | https://img.tarotnow.xyz/light-god-50/21_Judgement_50_20260325_181359.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1095 | 123 | third-party | https://img.tarotnow.xyz/light-god-50/04_The_Empress_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1056 | 308 | third-party | https://img.tarotnow.xyz/light-god-50/13_The_Hanged_Man_50_20260325_181356.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1022 | 142 | third-party | https://img.tarotnow.xyz/light-god-50/19_The_Moon_50_20260325_181359.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1019 | 115 | third-party | https://img.tarotnow.xyz/light-god-50/20_The_Sun_50_20260325_181359.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | auth-public | logged-out | mobile | /vi | GET | 200 | 1008 | 635 | html | https://www.tarotnow.xyz/vi |
| Critical | auth-public | logged-out | desktop | /vi | GET | 200 | 1007 | 690 | html | https://www.tarotnow.xyz/vi |
| Critical | auth-public | logged-out | desktop | /vi/verify-email | GET | 200 | 973 | 373 | html | https://www.tarotnow.xyz/vi/verify-email |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 960 | 282 | third-party | https://img.tarotnow.xyz/light-god-50/12_Justice_50_20260325_181353.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 948 | 134 | third-party | https://img.tarotnow.xyz/light-god-50/18_The_Star_50_20260325_181357.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | reading | logged-in-admin | mobile | /vi/reading | GET | 200 | 939 | 935 | api | https://www.tarotnow.xyz/api/auth/session |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 920 | 141 | third-party | https://img.tarotnow.xyz/light-god-50/17_The_Tower_50_20260325_181357.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/withdraw | GET | 200 | 892 | 335 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 881 | 296 | third-party | https://img.tarotnow.xyz/light-god-50/11_Wheel_of%20_Fortune_50_20260325_181353.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | reader-chat | logged-in-reader | desktop | /vi/readers | GET | 200 | 874 | 331 | html | https://www.tarotnow.xyz/vi/readers |
| Critical | reader-chat | logged-in-admin | mobile | /vi/chat | GET | 200 | 874 | 395 | html | https://www.tarotnow.xyz/vi/chat |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet | GET | 200 | 872 | 342 | html | https://www.tarotnow.xyz/vi/wallet |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | GET | 200 | 869 | 335 | html | https://www.tarotnow.xyz/vi/profile |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha | GET | 200 | 869 | 629 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Frare_title_lucky_star_50_20260416_165453.avif&w=256&q=75 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | GET | 200 | 868 | 336 | html | https://www.tarotnow.xyz/vi/wallet |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 861 | 357 | html | https://www.tarotnow.xyz/vi/collection |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers | GET | 200 | 847 | 335 | html | https://www.tarotnow.xyz/vi/readers |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | GET | 200 | 846 | 836 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Frare_title_lucky_star_50_20260416_165453.avif&w=48&q=75 |
| Critical | community-leaderboard-quest | logged-in-admin | mobile | /vi/leaderboard | GET | 200 | 841 | 390 | html | https://www.tarotnow.xyz/vi/leaderboard |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/notifications | GET | 200 | 839 | 323 | html | https://www.tarotnow.xyz/vi/notifications |
| Critical | admin | logged-in-admin | desktop | /vi/admin/deposits | GET | 200 | 838 | 451 | html | https://www.tarotnow.xyz/vi/admin/deposits |
| Critical | reading | logged-in-admin | desktop | /vi/reading | GET | 200 | 830 | 374 | html | https://www.tarotnow.xyz/vi/reading |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers | GET | 200 | 830 | 340 | html | https://www.tarotnow.xyz/vi/readers |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | GET | 200 | 825 | 351 | html | https://www.tarotnow.xyz/vi/gamification |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/inventory | GET | 200 | 825 | 336 | html | https://www.tarotnow.xyz/vi/inventory |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/notifications | GET | 200 | 825 | 317 | html | https://www.tarotnow.xyz/vi/notifications |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers | GET | 200 | 824 | 310 | html | https://www.tarotnow.xyz/vi/readers |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/gamification | GET | 200 | 821 | 338 | html | https://www.tarotnow.xyz/vi/gamification |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha | GET | 200 | 818 | 324 | html | https://www.tarotnow.xyz/vi/gacha |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet | GET | 200 | 818 | 331 | html | https://www.tarotnow.xyz/vi/wallet |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | GET | 200 | 814 | 354 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet | GET | 200 | 812 | 330 | html | https://www.tarotnow.xyz/vi/wallet |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile | GET | 200 | 811 | 332 | html | https://www.tarotnow.xyz/vi/profile |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha/history | GET | 200 | 810 | 321 | html | https://www.tarotnow.xyz/vi/gacha/history |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile | GET | 200 | 806 | 375 | html | https://www.tarotnow.xyz/vi/profile |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha | GET | 200 | 804 | 332 | html | https://www.tarotnow.xyz/vi/gacha |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | GET | 200 | 803 | 339 | html | https://www.tarotnow.xyz/vi/inventory |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha | GET | 200 | 803 | 318 | html | https://www.tarotnow.xyz/vi/gacha |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit/history | GET | 200 | 801 | 341 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| Critical | reading | logged-in-reader | desktop | /vi/reading | GET | 200 | 801 | 328 | html | https://www.tarotnow.xyz/vi/reading |
| Medium | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | GET | 200 | 799 | 329 | html | https://www.tarotnow.xyz/vi/gamification |

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
