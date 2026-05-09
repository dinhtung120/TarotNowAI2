# PERFORMANCE AUDIT - TarotNow

## Executive Summary

- Generated at (UTC): 2026-05-09T00:45:47.381Z
- Benchmark generated at (UTC): 2026-05-09T00:45:40.487Z
- Benchmark input: Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark.json
- Base origin: https://www.tarotnow.xyz
- Locale prefix: /vi
- Benchmark mode: full-matrix
- Total scenarios: 6
- Total pages measured: 170
- Critical pages: 0
- High pages: 142
- Medium pages: 28
- Slow requests >800ms: 47
- Slow requests 400-800ms: 324
- Request thresholds: >25 suspicious, >35 severe
- Slow request thresholds: >400ms optimize, >800ms serious

## Scenario Coverage

| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Failed | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 24.9 | 2874 | 0 | 0 | yes |
| logged-in-admin | desktop | 43 | 28.8 | 3024 | 0 | 0 | yes |
| logged-in-reader | desktop | 33 | 28.7 | 3034 | 0 | 0 | yes |
| logged-out | mobile | 9 | 25.0 | 2766 | 0 | 0 | yes |
| logged-in-admin | mobile | 43 | 28.8 | 2944 | 0 | 0 | yes |
| logged-in-reader | mobile | 33 | 28.7 | 2954 | 0 | 0 | yes |

## Route Family Coverage

| Scenario | Viewport | Family | Pages | Avg requests/page | Avg nav (ms) | Pending | Failed |
| --- | --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.5 | 2935 | 0 | 0 |
| logged-in-admin | desktop | auth-public | 3 | 25.0 | 2803 | 0 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | 3 | 32.0 | 3547 | 0 | 0 |
| logged-in-admin | desktop | home | 1 | 26.0 | 2719 | 0 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | 4 | 30.8 | 4016 | 0 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | 8 | 28.4 | 2834 | 0 | 0 |
| logged-in-admin | desktop | reader-chat | 9 | 28.1 | 2861 | 0 | 0 |
| logged-in-admin | desktop | reading | 5 | 29.0 | 2889 | 0 | 0 |
| logged-in-admin | mobile | admin | 10 | 29.2 | 2842 | 0 | 0 |
| logged-in-admin | mobile | auth-public | 3 | 25.0 | 2736 | 0 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | 3 | 31.3 | 3140 | 0 | 0 |
| logged-in-admin | mobile | home | 1 | 26.0 | 2771 | 0 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | 4 | 31.5 | 3635 | 0 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | 8 | 28.4 | 2884 | 0 | 0 |
| logged-in-admin | mobile | reader-chat | 9 | 28.2 | 2879 | 0 | 0 |
| logged-in-admin | mobile | reading | 5 | 28.6 | 2855 | 0 | 0 |
| logged-in-reader | desktop | auth-public | 3 | 25.0 | 2751 | 0 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | 3 | 30.0 | 3158 | 0 | 0 |
| logged-in-reader | desktop | home | 1 | 26.0 | 2772 | 0 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | 4 | 31.0 | 3930 | 0 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | 8 | 28.6 | 2968 | 0 | 0 |
| logged-in-reader | desktop | reader-chat | 9 | 28.2 | 2827 | 0 | 0 |
| logged-in-reader | desktop | reading | 5 | 30.0 | 2944 | 0 | 0 |
| logged-in-reader | mobile | auth-public | 3 | 25.0 | 2726 | 0 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | 3 | 31.3 | 3112 | 0 | 0 |
| logged-in-reader | mobile | home | 1 | 26.0 | 2776 | 0 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | 4 | 30.5 | 3583 | 0 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | 8 | 28.8 | 2888 | 0 | 0 |
| logged-in-reader | mobile | reader-chat | 9 | 28.2 | 2841 | 0 | 0 |
| logged-in-reader | mobile | reading | 5 | 29.4 | 2841 | 0 | 0 |
| logged-out | desktop | auth-public | 8 | 24.4 | 2758 | 0 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 3803 | 0 | 0 |
| logged-out | mobile | auth-public | 8 | 24.4 | 2732 | 0 | 0 |
| logged-out | mobile | home | 1 | 30.0 | 3037 | 0 | 0 |

## Detailed Metrics Table

| Severity | Feature | Scenario | Viewport | Route | Requests | API | Static | Third-party | Nav (ms) | FCP (ms) | LCP (ms) | TBT (ms) | CLS | Pending | Failed | Transfer bytes |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/community | 35 | 1 | 31 | 1 | 4128 | 596 | 1380 | 0.0 | 0.0041 | 0 | 0 | 794709 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/inventory | 35 | 0 | 33 | 0 | 3024 | 560 | 1152 | 1.0 | 0.0037 | 0 | 0 | 651796 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha | 35 | 0 | 33 | 0 | 2858 | 488 | 848 | 0.0 | 0.0000 | 0 | 0 | 796544 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/community | 35 | 1 | 31 | 1 | 3751 | 516 | 1036 | 0.0 | 0.0000 | 0 | 0 | 794315 |
| High | community-leaderboard-quest | logged-in-reader | mobile | /vi/community | 35 | 1 | 31 | 1 | 3783 | 552 | 1336 | 0.0 | 0.0051 | 0 | 0 | 794466 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/6cb71599-47fa-459b-91b7-14194d917fdf | 34 | 2 | 30 | 0 | 3123 | 680 | 1112 | 0.0 | 0.0037 | 0 | 0 | 724662 |
| High | admin | logged-in-admin | desktop | /vi/admin/gamification | 33 | 1 | 30 | 0 | 3291 | 696 | 1112 | 0.0 | 0.0022 | 0 | 0 | 698938 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile | 33 | 3 | 27 | 0 | 3052 | 608 | 1104 | 0.0 | 0.0723 | 0 | 0 | 637207 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/7aec6104-c5de-46a4-829c-04d21e06aa4d | 33 | 2 | 29 | 0 | 2977 | 480 | 836 | 0.0 | 0.0000 | 0 | 0 | 693073 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha | 32 | 2 | 28 | 0 | 2931 | 580 | 1256 | 0.0 | 0.0041 | 0 | 0 | 726201 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha/history | 32 | 2 | 28 | 0 | 2996 | 560 | 1376 | 0.0 | 0.0041 | 0 | 0 | 727384 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/97d1c31e-faf4-4e03-b025-1cb780806f8a | 32 | 1 | 29 | 0 | 2943 | 544 | 976 | 0.0 | 0.0054 | 0 | 0 | 713143 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/474df09a-4a03-4cbc-8106-9c3b11fbe1f0 | 32 | 1 | 29 | 0 | 2907 | 540 | 908 | 0.0 | 0.0037 | 0 | 0 | 713374 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha | 32 | 2 | 28 | 0 | 2877 | 448 | 1104 | 0.0 | 0.0000 | 0 | 0 | 726294 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile | 32 | 2 | 27 | 0 | 3098 | 440 | 1080 | 0.0 | 0.0821 | 0 | 0 | 637212 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile | 31 | 2 | 27 | 0 | 3000 | 616 | 1012 | 0.0 | 0.0489 | 0 | 0 | 635425 |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/leaderboard | 31 | 1 | 28 | 0 | 3251 | 736 | 1148 | 0.0 | 0.0041 | 0 | 0 | 651124 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/inventory | 31 | 1 | 28 | 0 | 2886 | 460 | 1096 | 0.0 | 0.0000 | 0 | 0 | 644763 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha/history | 31 | 1 | 28 | 0 | 2886 | 452 | 872 | 0.0 | 0.0000 | 0 | 0 | 726413 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/c46e3f73-436d-4b30-8885-ab45de051571 | 31 | 1 | 28 | 0 | 2890 | 464 | 816 | 0.0 | 0.0000 | 0 | 0 | 681107 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/inventory | 31 | 1 | 28 | 0 | 2876 | 484 | 1120 | 0.0 | 0.0000 | 0 | 0 | 644782 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | 30 | 0 | 28 | 0 | 2852 | 520 | 1204 | 0.0 | 0.0041 | 0 | 0 | 644205 |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | 30 | 1 | 27 | 0 | 3263 | 528 | 968 | 0.0 | 0.0279 | 0 | 0 | 643160 |
| High | admin | logged-in-admin | desktop | /vi/admin/system-configs | 30 | 0 | 28 | 0 | 3012 | 588 | 1200 | 0.0 | 0.0000 | 0 | 0 | 688791 |
| High | admin | logged-in-admin | desktop | /vi/admin/users | 30 | 0 | 28 | 0 | 2764 | 520 | 944 | 0.0 | 0.0000 | 0 | 0 | 649432 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha | 30 | 0 | 28 | 0 | 2861 | 512 | 1196 | 0.0 | 0.0037 | 0 | 0 | 724586 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha/history | 30 | 0 | 28 | 0 | 2867 | 596 | 1392 | 0.0 | 0.0037 | 0 | 0 | 723703 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers | 30 | 2 | 26 | 0 | 2907 | 508 | 1168 | 0.0 | 0.0037 | 0 | 0 | 635276 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/leaderboard | 30 | 0 | 28 | 0 | 2904 | 564 | 1044 | 0.0 | 0.0037 | 0 | 0 | 650239 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/community | 30 | 0 | 28 | 0 | 3649 | 812 | 1344 | 0.0 | 0.0037 | 0 | 0 | 642509 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/gamification | 30 | 1 | 27 | 0 | 2922 | 536 | 1056 | 0.0 | 0.0274 | 0 | 0 | 643327 |
| High | auth-public | logged-out | mobile | /vi | 30 | 1 | 27 | 0 | 3037 | 596 | 960 | 0.0 | 0.0000 | 0 | 0 | 602123 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile | 30 | 1 | 27 | 0 | 3096 | 456 | 852 | 0.0 | 0.0000 | 0 | 0 | 634653 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/leaderboard | 30 | 0 | 28 | 0 | 2804 | 556 | 932 | 0.0 | 0.0000 | 0 | 0 | 650053 |
| High | admin | logged-in-admin | mobile | /vi/admin/gamification | 30 | 0 | 28 | 0 | 2872 | 652 | 980 | 0.0 | 0.0000 | 0 | 0 | 664447 |
| High | admin | logged-in-admin | mobile | /vi/admin/system-configs | 30 | 0 | 28 | 0 | 2930 | 504 | 852 | 0.0 | 0.0000 | 0 | 0 | 688909 |
| High | admin | logged-in-admin | mobile | /vi/admin/users | 30 | 0 | 28 | 0 | 2865 | 512 | 1156 | 0.0 | 0.0000 | 0 | 0 | 649486 |
| High | reading | logged-in-reader | mobile | /vi/reading | 30 | 2 | 26 | 0 | 2880 | 464 | 836 | 0.0 | 0.0000 | 0 | 0 | 642981 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha/history | 30 | 0 | 28 | 0 | 2890 | 464 | 1152 | 0.0 | 0.0000 | 0 | 0 | 723533 |
| High | community-leaderboard-quest | logged-in-reader | mobile | /vi/leaderboard | 30 | 0 | 28 | 0 | 2688 | 436 | 800 | 0.0 | 0.0000 | 0 | 0 | 649850 |
| High | auth-public | logged-out | desktop | /vi | 29 | 0 | 27 | 0 | 3803 | 1324 | 1324 | 651.0 | 0.0000 | 0 | 0 | 600862 |
| High | reading | logged-in-admin | desktop | /vi/reading | 29 | 1 | 26 | 0 | 2930 | 500 | 992 | 0.0 | 0.0041 | 0 | 0 | 642302 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | 29 | 0 | 27 | 0 | 7284 | 588 | 588 | 169.0 | 0.0042 | 0 | 0 | 642758 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers | 29 | 1 | 26 | 0 | 2885 | 524 | 1212 | 0.0 | 0.0041 | 0 | 0 | 634377 |
| High | admin | logged-in-admin | desktop | /vi/admin | 29 | 0 | 27 | 0 | 3228 | 680 | 1116 | 0.0 | 0.0000 | 0 | 0 | 647007 |
| High | admin | logged-in-admin | desktop | /vi/admin/deposits | 29 | 0 | 27 | 0 | 2736 | 552 | 1156 | 0.0 | 0.0000 | 0 | 0 | 647106 |
| High | admin | logged-in-admin | desktop | /vi/admin/disputes | 29 | 0 | 27 | 0 | 2821 | 632 | 976 | 0.0 | 0.0000 | 0 | 0 | 645376 |
| High | admin | logged-in-admin | desktop | /vi/admin/reader-requests | 29 | 0 | 27 | 0 | 2894 | 568 | 972 | 0.0 | 0.0000 | 0 | 0 | 646120 |
| High | admin | logged-in-admin | desktop | /vi/admin/readings | 29 | 0 | 27 | 0 | 2903 | 608 | 1208 | 0.0 | 0.0000 | 0 | 0 | 648231 |
| High | admin | logged-in-admin | desktop | /vi/admin/withdrawals | 29 | 0 | 27 | 0 | 2811 | 564 | 936 | 0.0 | 0.0000 | 0 | 0 | 645587 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | 29 | 0 | 27 | 0 | 6969 | 504 | 992 | 0.0 | 0.0037 | 0 | 0 | 641693 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | 29 | 0 | 27 | 0 | 5910 | 452 | 452 | 16.0 | 0.0000 | 0 | 0 | 642867 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers | 29 | 1 | 26 | 0 | 2910 | 464 | 1096 | 0.0 | 0.0000 | 0 | 0 | 634081 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 29 | 0 | 27 | 0 | 2864 | 464 | 848 | 0.0 | 0.0000 | 0 | 0 | 641950 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet | 29 | 1 | 26 | 0 | 2932 | 492 | 1168 | 0.0 | 0.0000 | 0 | 0 | 634205 |
| High | admin | logged-in-admin | mobile | /vi/admin | 29 | 0 | 27 | 0 | 2772 | 500 | 832 | 0.0 | 0.0000 | 0 | 0 | 647087 |
| High | admin | logged-in-admin | mobile | /vi/admin/deposits | 29 | 0 | 27 | 0 | 2834 | 488 | 884 | 0.0 | 0.0000 | 0 | 0 | 647163 |
| High | admin | logged-in-admin | mobile | /vi/admin/disputes | 29 | 0 | 27 | 0 | 2845 | 556 | 864 | 0.0 | 0.0000 | 0 | 0 | 645391 |
| High | admin | logged-in-admin | mobile | /vi/admin/reader-requests | 29 | 0 | 27 | 0 | 2736 | 456 | 784 | 0.0 | 0.0000 | 0 | 0 | 645905 |
| High | admin | logged-in-admin | mobile | /vi/admin/readings | 29 | 0 | 27 | 0 | 2903 | 520 | 892 | 0.0 | 0.0000 | 0 | 0 | 648116 |
| High | admin | logged-in-admin | mobile | /vi/admin/withdrawals | 29 | 0 | 27 | 0 | 2884 | 480 | 840 | 0.0 | 0.0000 | 0 | 0 | 645536 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 29 | 1 | 26 | 0 | 2877 | 460 | 812 | 0.0 | 0.0000 | 0 | 0 | 631374 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | 29 | 0 | 27 | 0 | 5688 | 476 | 476 | 0.0 | 0.0000 | 0 | 0 | 641665 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/mfa | 29 | 1 | 26 | 0 | 2899 | 480 | 872 | 0.0 | 0.0000 | 0 | 0 | 631707 |
| High | community-leaderboard-quest | logged-in-reader | mobile | /vi/gamification | 29 | 0 | 27 | 0 | 2865 | 452 | 824 | 0.0 | 0.0000 | 0 | 0 | 642193 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet | 29 | 1 | 26 | 0 | 2875 | 452 | 1160 | 0.0 | 0.0000 | 0 | 0 | 634354 |
| High | reader-chat | logged-in-reader | mobile | /vi/reader/apply | 29 | 1 | 26 | 0 | 2940 | 472 | 912 | 0.0 | 0.0000 | 0 | 0 | 632516 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 29 | 1 | 26 | 0 | 2891 | 476 | 852 | 0.0 | 0.0000 | 0 | 0 | 631419 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | 28 | 0 | 26 | 0 | 2808 | 632 | 1020 | 0.0 | 0.0041 | 0 | 0 | 631199 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | 28 | 0 | 26 | 0 | 2788 | 888 | 1224 | 0.0 | 0.0489 | 0 | 0 | 630429 |
| High | reader-chat | logged-in-admin | desktop | /vi/chat | 28 | 0 | 26 | 0 | 2866 | 540 | 1020 | 0.0 | 0.0041 | 0 | 0 | 631310 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet | 28 | 0 | 26 | 0 | 2874 | 568 | 1356 | 0.0 | 0.0041 | 0 | 0 | 633465 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 2758 | 528 | 992 | 0.0 | 0.0041 | 0 | 0 | 630968 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 2822 | 608 | 1328 | 0.0 | 0.0041 | 0 | 0 | 631912 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | 28 | 0 | 26 | 0 | 2778 | 524 | 1268 | 0.0 | 0.0041 | 0 | 0 | 630605 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/notifications | 28 | 0 | 26 | 0 | 2840 | 556 | 988 | 0.0 | 0.0046 | 0 | 0 | 631627 |
| High | reader-chat | logged-in-admin | desktop | /vi/reader/apply | 28 | 0 | 26 | 0 | 3196 | 1076 | 1492 | 0.0 | 0.0041 | 0 | 0 | 631964 |
| High | reading | logged-in-admin | desktop | /vi/reading/history | 28 | 0 | 26 | 0 | 2758 | 556 | 1320 | 0.0 | 0.0041 | 0 | 0 | 632243 |
| High | admin | logged-in-admin | desktop | /vi/admin/promotions | 28 | 0 | 26 | 0 | 2888 | 584 | 936 | 0.0 | 0.0000 | 0 | 0 | 644181 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/29bf38b4-07f7-44b7-9de9-51487ad4e9ea | 28 | 0 | 26 | 0 | 3069 | 912 | 1284 | 0.0 | 0.0041 | 0 | 0 | 631909 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/2f301395-d111-4dff-98d5-349169638fd3 | 28 | 0 | 26 | 0 | 2745 | 648 | 1036 | 0.0 | 0.0041 | 0 | 0 | 631908 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 2813 | 552 | 960 | 0.0 | 0.0041 | 0 | 0 | 630825 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 2822 | 568 | 984 | 0.0 | 0.0041 | 0 | 0 | 630891 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 2914 | 516 | 984 | 0.0 | 0.0041 | 0 | 0 | 630547 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 2699 | 484 | 1132 | 0.0 | 0.0041 | 0 | 0 | 632296 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 2735 | 536 | 1236 | 0.0 | 0.0041 | 0 | 0 | 632365 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 2815 | 720 | 1440 | 0.0 | 0.0041 | 0 | 0 | 632468 |
| High | reading | logged-in-reader | desktop | /vi/reading | 28 | 0 | 26 | 0 | 2920 | 588 | 976 | 0.0 | 0.0037 | 0 | 0 | 641337 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/mfa | 28 | 0 | 26 | 0 | 3129 | 1192 | 1192 | 0.0 | 0.0037 | 0 | 0 | 631321 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/reader | 28 | 0 | 26 | 0 | 2973 | 564 | 1400 | 0.0 | 0.0037 | 0 | 0 | 631943 |
| High | reader-chat | logged-in-reader | desktop | /vi/chat | 28 | 0 | 26 | 0 | 2722 | 536 | 988 | 0.0 | 0.0037 | 0 | 0 | 631130 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet | 28 | 0 | 26 | 0 | 3055 | 604 | 1428 | 0.0 | 0.0037 | 0 | 0 | 633675 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 2994 | 564 | 1088 | 0.0 | 0.0037 | 0 | 0 | 631254 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 2794 | 700 | 1108 | 0.0 | 0.0037 | 0 | 0 | 631126 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/withdraw | 28 | 0 | 26 | 0 | 2872 | 648 | 1112 | 0.0 | 0.0092 | 0 | 0 | 632606 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/notifications | 28 | 0 | 26 | 0 | 2871 | 520 | 1228 | 0.0 | 0.0037 | 0 | 0 | 631857 |
| High | reader-chat | logged-in-reader | desktop | /vi/reader/apply | 28 | 0 | 26 | 0 | 3029 | 584 | 1076 | 0.0 | 0.0037 | 0 | 0 | 632044 |
| High | reading | logged-in-reader | desktop | /vi/reading/history | 28 | 0 | 26 | 0 | 2902 | 564 | 1280 | 0.0 | 0.0037 | 0 | 0 | 632253 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/91aeb338-ac82-440d-af06-1be35041cea7 | 28 | 0 | 26 | 0 | 2867 | 612 | 1072 | 0.0 | 0.0049 | 0 | 0 | 631980 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 2751 | 524 | 920 | 0.0 | 0.0037 | 0 | 0 | 630723 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 2857 | 576 | 924 | 0.0 | 0.0037 | 0 | 0 | 630661 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 2774 | 520 | 932 | 0.0 | 0.0037 | 0 | 0 | 630795 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 2746 | 544 | 1252 | 0.0 | 0.0037 | 0 | 0 | 632213 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 2779 | 528 | 1212 | 0.0 | 0.0037 | 0 | 0 | 632671 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 2881 | 576 | 1204 | 0.0 | 0.0037 | 0 | 0 | 632846 |
| High | reading | logged-in-admin | mobile | /vi/reading | 28 | 0 | 26 | 0 | 2871 | 624 | 984 | 0.0 | 0.0000 | 0 | 0 | 641640 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/mfa | 28 | 0 | 26 | 0 | 2864 | 496 | 840 | 0.0 | 0.0000 | 0 | 0 | 631098 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | 28 | 0 | 26 | 0 | 2818 | 644 | 1004 | 0.0 | 0.0760 | 0 | 0 | 630262 |
| High | reader-chat | logged-in-admin | mobile | /vi/chat | 28 | 0 | 26 | 0 | 2962 | 672 | 1000 | 0.0 | 0.0000 | 0 | 0 | 631038 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 2821 | 544 | 888 | 0.0 | 0.0000 | 0 | 0 | 631077 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 2938 | 508 | 892 | 0.0 | 0.0000 | 0 | 0 | 631920 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 28 | 0 | 26 | 0 | 2875 | 696 | 1120 | 0.0 | 0.0071 | 0 | 0 | 630546 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/notifications | 28 | 0 | 26 | 0 | 2724 | 480 | 832 | 0.0 | 0.0000 | 0 | 0 | 631638 |
| High | reader-chat | logged-in-admin | mobile | /vi/reader/apply | 28 | 0 | 26 | 0 | 2930 | 480 | 884 | 0.0 | 0.0000 | 0 | 0 | 631898 |
| High | reading | logged-in-admin | mobile | /vi/reading/history | 28 | 0 | 26 | 0 | 2722 | 448 | 808 | 0.0 | 0.0000 | 0 | 0 | 632523 |
| High | admin | logged-in-admin | mobile | /vi/admin/promotions | 28 | 0 | 26 | 0 | 2778 | 476 | 812 | 0.0 | 0.0000 | 0 | 0 | 644115 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/b098803d-a599-4222-b462-44ecb8eaffe7 | 28 | 0 | 26 | 0 | 2909 | 736 | 1076 | 0.0 | 0.0000 | 0 | 0 | 631860 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/efe733dd-c6da-4d12-86b4-eb915ab46895 | 28 | 0 | 26 | 0 | 2881 | 588 | 940 | 0.0 | 0.0000 | 0 | 0 | 631782 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 2757 | 452 | 780 | 0.0 | 0.0000 | 0 | 0 | 630689 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 2784 | 492 | 812 | 0.0 | 0.0000 | 0 | 0 | 630736 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 2823 | 448 | 812 | 0.0 | 0.0000 | 0 | 0 | 632200 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 2955 | 548 | 916 | 0.0 | 0.0000 | 0 | 0 | 632287 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 2909 | 628 | 996 | 0.0 | 0.0000 | 0 | 0 | 632636 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/reader | 28 | 0 | 26 | 0 | 2842 | 484 | 828 | 0.0 | 0.0000 | 0 | 0 | 631765 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers | 28 | 0 | 26 | 0 | 2908 | 512 | 900 | 0.0 | 0.0000 | 0 | 0 | 633428 |
| High | reader-chat | logged-in-reader | mobile | /vi/chat | 28 | 0 | 26 | 0 | 2823 | 524 | 868 | 0.0 | 0.0000 | 0 | 0 | 631176 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 2735 | 496 | 848 | 0.0 | 0.0000 | 0 | 0 | 631002 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 2873 | 516 | 848 | 0.0 | 0.0000 | 0 | 0 | 631398 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/withdraw | 28 | 0 | 26 | 0 | 2889 | 528 | 1176 | 0.0 | 0.0330 | 0 | 0 | 632688 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/notifications | 28 | 0 | 26 | 0 | 2891 | 528 | 884 | 0.0 | 0.0000 | 0 | 0 | 631824 |
| High | reading | logged-in-reader | mobile | /vi/reading/history | 28 | 0 | 26 | 0 | 2853 | 476 | 836 | 0.0 | 0.0000 | 0 | 0 | 632507 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/0a2d8e46-108f-4f9d-aa9f-2581183807ff | 28 | 0 | 26 | 0 | 2788 | 500 | 840 | 0.0 | 0.0000 | 0 | 0 | 631920 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/7ab89974-9ab4-4b27-ae8a-93965f8bfd71 | 28 | 0 | 26 | 0 | 2709 | 444 | 784 | 0.0 | 0.0000 | 0 | 0 | 631812 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 2752 | 568 | 892 | 0.0 | 0.0000 | 0 | 0 | 630753 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 2767 | 556 | 892 | 0.0 | 0.0000 | 0 | 0 | 630837 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 2820 | 456 | 808 | 0.0 | 0.0000 | 0 | 0 | 632355 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 2821 | 452 | 816 | 0.0 | 0.0000 | 0 | 0 | 632599 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 2845 | 484 | 832 | 0.0 | 0.0000 | 0 | 0 | 632827 |
| High | auth-public | logged-in-admin | desktop | /vi | 26 | 0 | 24 | 0 | 2719 | 680 | 1420 | 369.0 | 0.0039 | 0 | 0 | 537061 |
| High | auth-public | logged-in-reader | desktop | /vi | 26 | 0 | 24 | 0 | 2772 | 532 | 1260 | 403.0 | 0.0033 | 0 | 0 | 536997 |
| High | auth-public | logged-in-admin | mobile | /vi | 26 | 0 | 24 | 0 | 2771 | 580 | 948 | 0.0 | 0.0032 | 0 | 0 | 537111 |
| High | auth-public | logged-in-reader | mobile | /vi | 26 | 0 | 24 | 0 | 2776 | 496 | 916 | 0.0 | 0.0028 | 0 | 0 | 537097 |
| Medium | auth-public | logged-out | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 2735 | 500 | 832 | 0.0 | 0.0000 | 0 | 0 | 525276 |
| Medium | auth-public | logged-out | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2746 | 504 | 832 | 0.0 | 0.0000 | 0 | 0 | 525321 |
| Medium | auth-public | logged-out | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2707 | 496 | 824 | 0.0 | 0.0000 | 0 | 0 | 525360 |
| Medium | auth-public | logged-in-admin | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 2849 | 600 | 908 | 0.0 | 0.0020 | 0 | 0 | 525631 |
| Medium | auth-public | logged-in-admin | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2700 | 540 | 872 | 0.0 | 0.0020 | 0 | 0 | 525696 |
| Medium | auth-public | logged-in-admin | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2861 | 676 | 996 | 0.0 | 0.0020 | 0 | 0 | 525823 |
| Medium | auth-public | logged-in-reader | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 2794 | 668 | 944 | 0.0 | 0.0018 | 0 | 0 | 525678 |
| Medium | auth-public | logged-in-reader | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2728 | 560 | 876 | 0.0 | 0.0018 | 0 | 0 | 525590 |
| Medium | auth-public | logged-in-reader | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2730 | 520 | 836 | 0.0 | 0.0018 | 0 | 0 | 525817 |
| Medium | auth-public | logged-out | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 2769 | 448 | 820 | 0.0 | 0.0000 | 0 | 0 | 525326 |
| Medium | auth-public | logged-out | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2705 | 456 | 456 | 0.0 | 0.0000 | 0 | 0 | 525264 |
| Medium | auth-public | logged-out | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2819 | 472 | 472 | 0.0 | 0.0000 | 0 | 0 | 525425 |
| Medium | auth-public | logged-in-admin | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 2800 | 448 | 792 | 0.0 | 0.0032 | 0 | 0 | 525651 |
| Medium | auth-public | logged-in-admin | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2714 | 508 | 840 | 0.0 | 0.0032 | 0 | 0 | 525752 |
| Medium | auth-public | logged-in-admin | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2694 | 456 | 792 | 0.0 | 0.0032 | 0 | 0 | 525885 |
| Medium | auth-public | logged-in-reader | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 2682 | 476 | 792 | 0.0 | 0.0028 | 0 | 0 | 525619 |
| Medium | auth-public | logged-in-reader | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2689 | 480 | 816 | 0.0 | 0.0028 | 0 | 0 | 525627 |
| Medium | auth-public | logged-in-reader | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2807 | 528 | 860 | 0.0 | 0.0028 | 0 | 0 | 525653 |
| Medium | auth-public | logged-out | desktop | /vi/login | 24 | 0 | 22 | 0 | 2849 | 596 | 596 | 0.0 | 0.0000 | 0 | 0 | 511706 |
| Medium | auth-public | logged-out | desktop | /vi/register | 24 | 0 | 22 | 0 | 2869 | 700 | 700 | 0.0 | 0.0000 | 0 | 0 | 512303 |
| Medium | auth-public | logged-out | desktop | /vi/forgot-password | 24 | 0 | 22 | 0 | 2739 | 536 | 536 | 0.0 | 0.0000 | 0 | 0 | 511300 |
| Medium | auth-public | logged-out | desktop | /vi/reset-password | 24 | 0 | 22 | 0 | 2685 | 488 | 488 | 0.0 | 0.0000 | 0 | 0 | 511562 |
| Medium | auth-public | logged-out | desktop | /vi/verify-email | 24 | 0 | 22 | 0 | 2737 | 532 | 532 | 0.0 | 0.0000 | 0 | 0 | 511450 |
| Medium | auth-public | logged-out | mobile | /vi/login | 24 | 0 | 22 | 0 | 2699 | 636 | 636 | 0.0 | 0.0000 | 0 | 0 | 511712 |
| Medium | auth-public | logged-out | mobile | /vi/register | 24 | 0 | 22 | 0 | 2728 | 484 | 484 | 0.0 | 0.0000 | 0 | 0 | 512287 |
| Medium | auth-public | logged-out | mobile | /vi/forgot-password | 24 | 0 | 22 | 0 | 2743 | 484 | 484 | 0.0 | 0.0000 | 0 | 0 | 511290 |
| Medium | auth-public | logged-out | mobile | /vi/reset-password | 24 | 0 | 22 | 0 | 2647 | 456 | 456 | 0.0 | 0.0000 | 0 | 0 | 511488 |
| Medium | auth-public | logged-out | mobile | /vi/verify-email | 24 | 0 | 22 | 0 | 2749 | 468 | 468 | 0.0 | 0.0000 | 0 | 0 | 511532 |

## Major Issues Found

- Critical: chưa phát hiện page Critical theo benchmark hiện tại.
- High: 142 page(s) vượt ngưỡng >25 requests hoặc có request >800ms.
- Medium: 28 page(s) có request trong dải 400-800ms.
- Duplicate: chưa phát hiện duplicate request business đáng kể.
- Pending: chưa phát hiện pending request bất thường.

### Slow Requests

| Severity | Feature | Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1328 | 330 | third-party | https://img.tarotnow.xyz/light-god-50/07_The_Lovers_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1326 | 251 | third-party | https://img.tarotnow.xyz/light-god-50/08_The_Chariot_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1312 | 138 | third-party | https://img.tarotnow.xyz/light-god-50/08_The_Chariot_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1274 | 199 | third-party | https://img.tarotnow.xyz/light-god-50/07_The_Lovers_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1113 | 116 | third-party | https://img.tarotnow.xyz/light-god-50/05_The_Emperor_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1098 | 114 | third-party | https://img.tarotnow.xyz/light-god-50/06_The_Hierophant_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | reader-chat | logged-in-admin | desktop | /vi/reader/apply | GET | 200 | 1095 | 343 | html | https://www.tarotnow.xyz/vi/reader/apply |
| Critical | auth-public | logged-out | desktop | /vi | GET | 200 | 1046 | 538 | static | https://www.tarotnow.xyz/_next/static/chunks/0b8hzxtf1-_dk.js |
| Critical | auth-public | logged-out | desktop | /vi | GET | 200 | 1043 | 568 | static | https://www.tarotnow.xyz/_next/static/chunks/12oo753jmxg~2.js |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1036 | 117 | third-party | https://img.tarotnow.xyz/light-god-50/04_The_Empress_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | auth-public | logged-out | desktop | /vi | GET | 200 | 1030 | 357 | html | https://www.tarotnow.xyz/vi |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 996 | 114 | third-party | https://img.tarotnow.xyz/light-god-50/05_The_Emperor_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet | GET | 200 | 955 | 392 | html | https://www.tarotnow.xyz/vi/wallet |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/29bf38b4-07f7-44b7-9de9-51487ad4e9ea | GET | 200 | 954 | 323 | html | https://www.tarotnow.xyz/vi/reading/session/29bf38b4-07f7-44b7-9de9-51487ad4e9ea |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/mfa | GET | 200 | 950 | 331 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha | GET | 200 | 932 | 681 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Frare_title_lucky_star_50_20260416_165453.avif&w=256&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 928 | 114 | third-party | https://img.tarotnow.xyz/light-god-50/04_The_Empress_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha/history | GET | 200 | 913 | 322 | html | https://www.tarotnow.xyz/vi/gacha/history |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/inventory | GET | 200 | 909 | 325 | html | https://www.tarotnow.xyz/vi/inventory |
| Critical | reader-chat | logged-in-reader | desktop | /vi/reader/apply | GET | 200 | 893 | 318 | html | https://www.tarotnow.xyz/vi/reader/apply |
| Critical | reader-chat | logged-in-admin | mobile | /vi/chat | GET | 200 | 882 | 542 | html | https://www.tarotnow.xyz/vi/chat |
| Critical | reading | logged-in-reader | desktop | /vi/reading/session/6cb71599-47fa-459b-91b7-14194d917fdf | GET | 200 | 866 | 319 | html | https://www.tarotnow.xyz/vi/reading/session/6cb71599-47fa-459b-91b7-14194d917fdf |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha | GET | 200 | 851 | 317 | html | https://www.tarotnow.xyz/vi/gacha |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 850 | 104 | third-party | https://img.tarotnow.xyz/light-god-50/07_The_Lovers_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 843 | 354 | html | https://www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |
| Critical | reader-chat | logged-in-reader | desktop | /vi/readers | GET | 200 | 838 | 319 | html | https://www.tarotnow.xyz/vi/readers |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/community | GET | 200 | 838 | 319 | html | https://www.tarotnow.xyz/vi/community |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | GET | 200 | 833 | 827 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Frare_title_lucky_star_50_20260416_165453.avif&w=48&q=75 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers | GET | 200 | 833 | 326 | html | https://www.tarotnow.xyz/vi/readers |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet | GET | 200 | 832 | 324 | html | https://www.tarotnow.xyz/vi/wallet |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 829 | 117 | third-party | https://img.tarotnow.xyz/light-god-50/06_The_Hierophant_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/reader | GET | 200 | 829 | 333 | html | https://www.tarotnow.xyz/vi/profile/reader |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | GET | 200 | 823 | 346 | html | https://www.tarotnow.xyz/vi/profile |
| Critical | reader-chat | logged-in-reader | mobile | /vi/reader/apply | GET | 200 | 822 | 323 | html | https://www.tarotnow.xyz/vi/reader/apply |
| Critical | admin | logged-in-admin | desktop | /vi/admin/system-configs | GET | 200 | 820 | 356 | html | https://www.tarotnow.xyz/vi/admin/system-configs |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit/history | GET | 200 | 820 | 355 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/inventory | GET | 200 | 818 | 327 | html | https://www.tarotnow.xyz/vi/inventory |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/withdraw | GET | 200 | 818 | 339 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| Critical | community-leaderboard-quest | logged-in-reader | desktop | /vi/gamification | GET | 200 | 814 | 327 | html | https://www.tarotnow.xyz/vi/gamification |
| Critical | community-leaderboard-quest | logged-in-reader | desktop | /vi/community | GET | 200 | 813 | 377 | html | https://www.tarotnow.xyz/vi/community |
| Critical | reader-chat | logged-in-admin | mobile | /vi/reader/apply | GET | 200 | 812 | 313 | html | https://www.tarotnow.xyz/vi/reader/apply |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/community | GET | 200 | 805 | 350 | html | https://www.tarotnow.xyz/vi/community |
| Critical | admin | logged-in-admin | mobile | /vi/admin/readings | GET | 200 | 805 | 325 | html | https://www.tarotnow.xyz/vi/admin/readings |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | GET | 200 | 802 | 313 | html | https://www.tarotnow.xyz/vi/profile |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/inventory | GET | 200 | 802 | 319 | html | https://www.tarotnow.xyz/vi/inventory |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha/history | GET | 200 | 801 | 320 | html | https://www.tarotnow.xyz/vi/gacha/history |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/efe733dd-c6da-4d12-86b4-eb915ab46895 | GET | 200 | 801 | 317 | html | https://www.tarotnow.xyz/vi/reading/session/efe733dd-c6da-4d12-86b4-eb915ab46895 |
| Medium | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | GET | 200 | 800 | 313 | html | https://www.tarotnow.xyz/vi/wallet |
| Medium | admin | logged-in-admin | mobile | /vi/admin/withdrawals | GET | 200 | 799 | 335 | html | https://www.tarotnow.xyz/vi/admin/withdrawals |
| Medium | reading | logged-in-admin | desktop | /vi/reading | GET | 200 | 798 | 323 | html | https://www.tarotnow.xyz/vi/reading |
| Medium | reader-chat | logged-in-admin | desktop | /vi/readers | GET | 200 | 798 | 330 | html | https://www.tarotnow.xyz/vi/readers |
| Medium | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha/history | GET | 200 | 797 | 317 | html | https://www.tarotnow.xyz/vi/gacha/history |
| Medium | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | GET | 200 | 795 | 322 | html | https://www.tarotnow.xyz/vi/wallet |
| Medium | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/withdraw | GET | 200 | 794 | 315 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| Medium | reading | logged-in-admin | mobile | /vi/reading | GET | 200 | 793 | 321 | html | https://www.tarotnow.xyz/vi/reading |
| Medium | reader-chat | logged-in-reader | mobile | /vi/readers | GET | 200 | 793 | 318 | html | https://www.tarotnow.xyz/vi/readers |
| Medium | admin | logged-in-admin | desktop | /vi/admin | GET | 200 | 792 | 540 | static | https://www.tarotnow.xyz/_next/static/chunks/0mp2dyp5p7fbx.js |
| Medium | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile | GET | 200 | 792 | 315 | html | https://www.tarotnow.xyz/vi/profile |
| Medium | reader-chat | logged-in-admin | desktop | /vi/chat | GET | 200 | 791 | 338 | html | https://www.tarotnow.xyz/vi/chat |
| Medium | reader-chat | logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 791 | 304 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| Medium | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 791 | 69 | third-party | https://img.tarotnow.xyz/light-god-50/03_The_High%20Priestess%20_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| Medium | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha | GET | 200 | 790 | 320 | html | https://www.tarotnow.xyz/vi/gacha |
| Medium | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet | GET | 200 | 789 | 315 | html | https://www.tarotnow.xyz/vi/wallet |
| Medium | reading | logged-in-reader | desktop | /vi/reading | GET | 200 | 789 | 316 | html | https://www.tarotnow.xyz/vi/reading |
| Medium | profile-wallet-notifications | logged-in-reader | mobile | /vi/notifications | GET | 200 | 789 | 379 | html | https://www.tarotnow.xyz/vi/notifications |
| Medium | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | GET | 200 | 786 | 324 | html | https://www.tarotnow.xyz/vi/gamification |
| Medium | admin | logged-in-admin | mobile | /vi/admin/gamification | GET | 200 | 783 | 326 | html | https://www.tarotnow.xyz/vi/admin/gamification |
| Medium | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha/history | GET | 200 | 782 | 306 | html | https://www.tarotnow.xyz/vi/gacha/history |
| Medium | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet | GET | 200 | 781 | 303 | html | https://www.tarotnow.xyz/vi/wallet |
| Medium | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha | GET | 200 | 780 | 312 | html | https://www.tarotnow.xyz/vi/gacha |
| Medium | admin | logged-in-admin | desktop | /vi/admin | GET | 200 | 779 | 520 | static | https://www.tarotnow.xyz/_next/static/chunks/0f8m2dxx~fr_z.js |
| Medium | profile-wallet-notifications | logged-in-reader | desktop | /vi/notifications | GET | 200 | 779 | 307 | html | https://www.tarotnow.xyz/vi/notifications |
| Medium | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | GET | 200 | 778 | 324 | html | https://www.tarotnow.xyz/vi/gamification |
| Medium | reading | logged-in-reader | mobile | /vi/reading | GET | 200 | 777 | 311 | html | https://www.tarotnow.xyz/vi/reading |
| Medium | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 776 | 319 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| Medium | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile | GET | 200 | 775 | 307 | html | https://www.tarotnow.xyz/vi/profile |
| Medium | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | GET | 200 | 774 | 342 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| Medium | reading | logged-in-admin | mobile | /vi/reading/session/b098803d-a599-4222-b462-44ecb8eaffe7 | GET | 200 | 773 | 399 | html | https://www.tarotnow.xyz/vi/reading/session/b098803d-a599-4222-b462-44ecb8eaffe7 |
| Medium | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | GET | 200 | 772 | 317 | html | https://www.tarotnow.xyz/vi/inventory |
| Medium | admin | logged-in-admin | mobile | /vi/admin/users | GET | 200 | 772 | 335 | html | https://www.tarotnow.xyz/vi/admin/users |

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
