# PERFORMANCE AUDIT - TarotNow

## Executive Summary

- Generated at (UTC): 2026-05-09T10:57:24.485Z
- Benchmark generated at (UTC): 2026-05-09T01:19:17.648Z
- Benchmark input: Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark.json
- Base origin: https://www.tarotnow.xyz
- Locale prefix: /vi
- Benchmark mode: full-matrix
- Total scenarios: 6
- Total pages measured: 170
- Critical pages: 0
- High pages: 143
- Medium pages: 27
- Slow requests >800ms: 50
- Slow requests 400-800ms: 313
- Request thresholds: >25 suspicious, >35 severe
- Slow request thresholds: >400ms optimize, >800ms serious

## Scenario Coverage

| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Failed | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 24.9 | 2893 | 0 | 0 | yes |
| logged-in-admin | desktop | 43 | 28.9 | 3027 | 0 | 0 | yes |
| logged-in-reader | desktop | 33 | 28.7 | 3003 | 0 | 0 | yes |
| logged-out | mobile | 9 | 24.6 | 2782 | 0 | 0 | yes |
| logged-in-admin | mobile | 43 | 29.1 | 2949 | 0 | 0 | yes |
| logged-in-reader | mobile | 33 | 29.1 | 3023 | 0 | 0 | yes |

## Route Family Coverage

| Scenario | Viewport | Family | Pages | Avg requests/page | Avg nav (ms) | Pending | Failed |
| --- | --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.3 | 2900 | 0 | 0 |
| logged-in-admin | desktop | auth-public | 3 | 25.0 | 2724 | 0 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | 3 | 32.0 | 3583 | 0 | 0 |
| logged-in-admin | desktop | home | 1 | 26.0 | 2776 | 0 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | 4 | 30.5 | 4040 | 0 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | 8 | 28.4 | 2913 | 0 | 0 |
| logged-in-admin | desktop | reader-chat | 9 | 28.0 | 2853 | 0 | 0 |
| logged-in-admin | desktop | reading | 5 | 30.2 | 2863 | 0 | 0 |
| logged-in-admin | mobile | admin | 10 | 29.6 | 2903 | 0 | 0 |
| logged-in-admin | mobile | auth-public | 3 | 25.0 | 2748 | 0 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | 3 | 31.3 | 3116 | 0 | 0 |
| logged-in-admin | mobile | home | 1 | 26.0 | 2777 | 0 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | 4 | 30.5 | 3619 | 0 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | 8 | 28.5 | 2825 | 0 | 0 |
| logged-in-admin | mobile | reader-chat | 9 | 28.3 | 2857 | 0 | 0 |
| logged-in-admin | mobile | reading | 5 | 31.2 | 2922 | 0 | 0 |
| logged-in-reader | desktop | auth-public | 3 | 25.0 | 2837 | 0 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | 3 | 31.3 | 3149 | 0 | 0 |
| logged-in-reader | desktop | home | 1 | 26.0 | 2759 | 0 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | 4 | 31.0 | 3889 | 0 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | 8 | 28.6 | 2858 | 0 | 0 |
| logged-in-reader | desktop | reader-chat | 9 | 28.1 | 2851 | 0 | 0 |
| logged-in-reader | desktop | reading | 5 | 29.4 | 2860 | 0 | 0 |
| logged-in-reader | mobile | auth-public | 3 | 25.0 | 2761 | 0 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | 3 | 31.3 | 3192 | 0 | 0 |
| logged-in-reader | mobile | home | 1 | 26.0 | 2759 | 0 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | 4 | 32.5 | 3692 | 0 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | 8 | 28.9 | 2970 | 0 | 0 |
| logged-in-reader | mobile | reader-chat | 9 | 28.1 | 2890 | 0 | 0 |
| logged-in-reader | mobile | reading | 5 | 30.2 | 2921 | 0 | 0 |
| logged-out | desktop | auth-public | 8 | 24.4 | 2755 | 0 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 4002 | 0 | 0 |
| logged-out | mobile | auth-public | 8 | 24.4 | 2728 | 0 | 0 |
| logged-out | mobile | home | 1 | 26.0 | 3214 | 0 | 0 |

## Detailed Metrics Table

| Severity | Feature | Scenario | Viewport | Route | Requests | API | Static | Third-party | Nav (ms) | FCP (ms) | LCP (ms) | TBT (ms) | CLS | Pending | Failed | Transfer bytes |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/community | 35 | 1 | 31 | 1 | 4086 | 596 | 1176 | 0.0 | 0.0041 | 0 | 0 | 794699 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/inventory | 35 | 0 | 33 | 0 | 2905 | 572 | 1072 | 0.0 | 0.0037 | 0 | 0 | 651879 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/community | 35 | 1 | 31 | 1 | 3694 | 556 | 1100 | 0.0 | 0.0037 | 0 | 0 | 794465 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/community | 35 | 1 | 31 | 1 | 3667 | 484 | 996 | 0.0 | 0.0000 | 0 | 0 | 794136 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/inventory | 35 | 0 | 33 | 0 | 2942 | 528 | 908 | 0.0 | 0.0000 | 0 | 0 | 661218 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha | 35 | 0 | 33 | 0 | 2977 | 488 | 932 | 0.0 | 0.0069 | 0 | 0 | 796734 |
| High | community-leaderboard-quest | logged-in-reader | mobile | /vi/community | 35 | 1 | 31 | 1 | 3831 | 504 | 1208 | 1.0 | 0.0051 | 0 | 0 | 794463 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/949a0fbf-4b01-4b87-8037-37c80e86acc7 | 34 | 2 | 30 | 0 | 2973 | 568 | 976 | 0.0 | 0.0037 | 0 | 0 | 724700 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/513cf554-2eb5-4458-8c6f-acaf2685fca8 | 33 | 2 | 29 | 0 | 2933 | 528 | 940 | 0.0 | 0.0041 | 0 | 0 | 713994 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/dbbf695c-841c-43a9-87a6-6f163bb469cd | 33 | 2 | 29 | 0 | 2921 | 584 | 908 | 0.0 | 0.0051 | 0 | 0 | 714196 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/6f8210b4-a511-4ee4-99a7-e0730efe4828 | 33 | 2 | 29 | 0 | 2980 | 468 | 812 | 0.0 | 0.0000 | 0 | 0 | 692957 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/4d5d85d4-2afd-42ca-90f5-a4541495a069 | 33 | 2 | 29 | 0 | 2967 | 476 | 808 | 0.0 | 0.0000 | 0 | 0 | 692792 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/f481bbdc-f180-4075-b725-dfd8bd1ec4c3 | 33 | 2 | 29 | 0 | 3013 | 472 | 880 | 0.0 | 0.0000 | 0 | 0 | 692564 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile | 33 | 3 | 27 | 0 | 3773 | 512 | 1436 | 0.0 | 0.0889 | 0 | 0 | 638443 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/1c8022a9-71c4-4d9b-bdc7-7d42d9413c47 | 33 | 2 | 29 | 0 | 3017 | 472 | 856 | 0.0 | 0.0000 | 0 | 0 | 692980 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/b61cdc9c-52e7-4019-96b5-d60594bc7795 | 33 | 2 | 29 | 0 | 3065 | 504 | 880 | 0.0 | 0.0000 | 0 | 0 | 693116 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | 32 | 2 | 28 | 0 | 2971 | 528 | 1324 | 1.0 | 0.0041 | 0 | 0 | 645875 |
| High | admin | logged-in-admin | desktop | /vi/admin/gamification | 32 | 0 | 30 | 0 | 3299 | 532 | 908 | 0.0 | 0.0022 | 0 | 0 | 698078 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha | 32 | 2 | 28 | 0 | 2985 | 500 | 1148 | 0.0 | 0.0000 | 0 | 0 | 725846 |
| High | admin | logged-in-admin | mobile | /vi/admin/gamification | 32 | 0 | 30 | 0 | 2924 | 452 | 800 | 0.0 | 0.0000 | 0 | 0 | 697871 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha | 31 | 1 | 28 | 0 | 2952 | 584 | 1376 | 0.0 | 0.0041 | 0 | 0 | 725500 |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/leaderboard | 31 | 1 | 28 | 0 | 3384 | 652 | 1100 | 0.0 | 0.0041 | 0 | 0 | 650996 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/inventory | 31 | 1 | 28 | 0 | 2911 | 484 | 1116 | 0.0 | 0.0000 | 0 | 0 | 644657 |
| High | admin | logged-in-admin | mobile | /vi/admin/users | 31 | 0 | 29 | 0 | 2981 | 476 | 1184 | 0.0 | 0.0000 | 0 | 0 | 651031 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha/history | 31 | 1 | 28 | 0 | 2936 | 512 | 880 | 0.0 | 0.0000 | 0 | 0 | 724761 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha/history | 30 | 0 | 28 | 0 | 2908 | 528 | 1436 | 0.0 | 0.0041 | 0 | 0 | 725747 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile | 30 | 1 | 27 | 0 | 3035 | 532 | 1200 | 0.0 | 0.0489 | 0 | 0 | 634411 |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | 30 | 1 | 27 | 0 | 3280 | 532 | 1012 | 0.0 | 0.0279 | 0 | 0 | 643105 |
| High | admin | logged-in-admin | desktop | /vi/admin/readings | 30 | 0 | 28 | 0 | 2912 | 564 | 1076 | 0.0 | 0.0000 | 0 | 0 | 649833 |
| High | admin | logged-in-admin | desktop | /vi/admin/users | 30 | 0 | 28 | 0 | 2797 | 528 | 1224 | 0.0 | 0.0000 | 0 | 0 | 649469 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha | 30 | 0 | 28 | 0 | 2926 | 552 | 1264 | 0.0 | 0.0037 | 0 | 0 | 724664 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha/history | 30 | 0 | 28 | 0 | 2965 | 556 | 1488 | 0.0 | 0.0037 | 0 | 0 | 723424 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile | 30 | 1 | 27 | 0 | 3079 | 508 | 1220 | 0.0 | 0.0723 | 0 | 0 | 634452 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/leaderboard | 30 | 0 | 28 | 0 | 2871 | 540 | 924 | 0.0 | 0.0037 | 0 | 0 | 650117 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha/history | 30 | 0 | 28 | 0 | 2856 | 452 | 852 | 0.0 | 0.0000 | 0 | 0 | 725412 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile | 30 | 1 | 27 | 0 | 3062 | 476 | 1104 | 0.0 | 0.0689 | 0 | 0 | 634381 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/leaderboard | 30 | 0 | 28 | 0 | 2822 | 464 | 812 | 0.0 | 0.0000 | 0 | 0 | 650071 |
| High | admin | logged-in-admin | mobile | /vi/admin | 30 | 0 | 28 | 0 | 2907 | 480 | 824 | 0.0 | 0.0000 | 0 | 0 | 648567 |
| High | admin | logged-in-admin | mobile | /vi/admin/system-configs | 30 | 0 | 28 | 0 | 2917 | 476 | 828 | 0.0 | 0.0000 | 0 | 0 | 688767 |
| High | community-leaderboard-quest | logged-in-reader | mobile | /vi/leaderboard | 30 | 0 | 28 | 0 | 2868 | 488 | 836 | 0.0 | 0.0000 | 0 | 0 | 650191 |
| High | auth-public | logged-out | desktop | /vi | 29 | 0 | 27 | 0 | 4002 | 1412 | 1412 | 761.0 | 0.0000 | 0 | 0 | 600837 |
| High | reading | logged-in-admin | desktop | /vi/reading | 29 | 1 | 26 | 0 | 2964 | 548 | 1040 | 0.0 | 0.0041 | 0 | 0 | 642502 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | 29 | 0 | 27 | 0 | 7327 | 612 | 612 | 14.0 | 0.0042 | 0 | 0 | 642917 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet | 29 | 1 | 26 | 0 | 2945 | 592 | 1388 | 0.0 | 0.0041 | 0 | 0 | 634363 |
| High | admin | logged-in-admin | desktop | /vi/admin | 29 | 0 | 27 | 0 | 3193 | 568 | 928 | 0.0 | 0.0000 | 0 | 0 | 647168 |
| High | admin | logged-in-admin | desktop | /vi/admin/deposits | 29 | 0 | 27 | 0 | 2746 | 548 | 1088 | 0.0 | 0.0000 | 0 | 0 | 647097 |
| High | admin | logged-in-admin | desktop | /vi/admin/disputes | 29 | 0 | 27 | 0 | 2838 | 760 | 1116 | 0.0 | 0.0000 | 0 | 0 | 645430 |
| High | admin | logged-in-admin | desktop | /vi/admin/reader-requests | 29 | 0 | 27 | 0 | 2867 | 592 | 932 | 0.0 | 0.0000 | 0 | 0 | 646005 |
| High | admin | logged-in-admin | desktop | /vi/admin/withdrawals | 29 | 0 | 27 | 0 | 2804 | 564 | 840 | 0.0 | 0.0000 | 0 | 0 | 645550 |
| High | reading | logged-in-reader | desktop | /vi/reading | 29 | 1 | 26 | 0 | 2941 | 576 | 1040 | 0.0 | 0.0037 | 0 | 0 | 642521 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | 29 | 0 | 27 | 0 | 6761 | 520 | 908 | 5.0 | 0.0037 | 0 | 0 | 641813 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers | 29 | 1 | 26 | 0 | 2922 | 504 | 1268 | 0.0 | 0.0037 | 0 | 0 | 633905 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/gamification | 29 | 0 | 27 | 0 | 2882 | 532 | 1032 | 0.0 | 0.0274 | 0 | 0 | 642176 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet | 29 | 1 | 26 | 0 | 2893 | 536 | 1344 | 0.0 | 0.0037 | 0 | 0 | 634255 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/withdraw | 29 | 1 | 26 | 0 | 2932 | 548 | 1228 | 0.0 | 0.0092 | 0 | 0 | 633157 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/notifications | 29 | 1 | 26 | 0 | 2888 | 536 | 996 | 0.0 | 0.0037 | 0 | 0 | 632579 |
| High | reading | logged-in-admin | mobile | /vi/reading | 29 | 1 | 26 | 0 | 2911 | 496 | 848 | 0.0 | 0.0000 | 0 | 0 | 642073 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | 29 | 0 | 27 | 0 | 5724 | 460 | 460 | 5.0 | 0.0000 | 0 | 0 | 642651 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers | 29 | 1 | 26 | 0 | 2864 | 464 | 1096 | 0.0 | 0.0000 | 0 | 0 | 633832 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 29 | 0 | 27 | 0 | 2859 | 468 | 848 | 0.0 | 0.0000 | 0 | 0 | 642107 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet | 29 | 1 | 26 | 0 | 2881 | 484 | 1132 | 0.0 | 0.0000 | 0 | 0 | 634316 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit | 29 | 1 | 26 | 0 | 2883 | 468 | 852 | 0.0 | 0.0000 | 0 | 0 | 631590 |
| High | admin | logged-in-admin | mobile | /vi/admin/deposits | 29 | 0 | 27 | 0 | 2836 | 468 | 888 | 0.0 | 0.0000 | 0 | 0 | 647092 |
| High | admin | logged-in-admin | mobile | /vi/admin/disputes | 29 | 0 | 27 | 0 | 2840 | 496 | 816 | 0.0 | 0.0000 | 0 | 0 | 645504 |
| High | admin | logged-in-admin | mobile | /vi/admin/reader-requests | 29 | 0 | 27 | 0 | 2850 | 500 | 844 | 0.0 | 0.0000 | 0 | 0 | 645913 |
| High | admin | logged-in-admin | mobile | /vi/admin/readings | 29 | 0 | 27 | 0 | 2983 | 764 | 1108 | 0.0 | 0.0000 | 0 | 0 | 648214 |
| High | admin | logged-in-admin | mobile | /vi/admin/withdrawals | 29 | 0 | 27 | 0 | 2832 | 484 | 820 | 0.0 | 0.0000 | 0 | 0 | 645554 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | 29 | 1 | 26 | 0 | 2961 | 564 | 940 | 0.0 | 0.0000 | 0 | 0 | 631381 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 29 | 1 | 26 | 0 | 2898 | 496 | 876 | 0.0 | 0.0000 | 0 | 0 | 631177 |
| High | reading | logged-in-reader | mobile | /vi/reading | 29 | 1 | 26 | 0 | 2908 | 464 | 856 | 0.0 | 0.0000 | 0 | 0 | 642015 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | 29 | 0 | 27 | 0 | 5913 | 460 | 788 | 8.0 | 0.0000 | 0 | 0 | 641674 |
| High | community-leaderboard-quest | logged-in-reader | mobile | /vi/gamification | 29 | 0 | 27 | 0 | 2877 | 504 | 864 | 0.0 | 0.0000 | 0 | 0 | 642144 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit | 29 | 1 | 26 | 0 | 2877 | 496 | 880 | 0.0 | 0.0000 | 0 | 0 | 631820 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/withdraw | 29 | 1 | 26 | 0 | 2897 | 496 | 1128 | 0.0 | 0.0330 | 0 | 0 | 633190 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 29 | 1 | 26 | 0 | 3126 | 512 | 1080 | 0.0 | 0.0069 | 0 | 0 | 632967 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | 28 | 0 | 26 | 0 | 2908 | 684 | 1180 | 0.0 | 0.0041 | 0 | 0 | 631121 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | 28 | 0 | 26 | 0 | 2813 | 628 | 1052 | 0.0 | 0.0489 | 0 | 0 | 630586 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers | 28 | 0 | 26 | 0 | 2892 | 564 | 1220 | 0.0 | 0.0041 | 0 | 0 | 633655 |
| High | reader-chat | logged-in-admin | desktop | /vi/chat | 28 | 0 | 26 | 0 | 2866 | 556 | 1008 | 0.0 | 0.0041 | 0 | 0 | 631126 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 2792 | 540 | 980 | 0.0 | 0.0041 | 0 | 0 | 631027 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 2886 | 544 | 1200 | 0.0 | 0.0041 | 0 | 0 | 631902 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | 28 | 0 | 26 | 0 | 2953 | 596 | 1352 | 0.0 | 0.0041 | 0 | 0 | 630735 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/notifications | 28 | 0 | 26 | 0 | 2971 | 624 | 1060 | 0.0 | 0.0041 | 0 | 0 | 631698 |
| High | reader-chat | logged-in-admin | desktop | /vi/reader/apply | 28 | 0 | 26 | 0 | 3059 | 580 | 1164 | 0.0 | 0.0041 | 0 | 0 | 632131 |
| High | reading | logged-in-admin | desktop | /vi/reading/history | 28 | 0 | 26 | 0 | 2747 | 596 | 1300 | 0.0 | 0.0041 | 0 | 0 | 632505 |
| High | admin | logged-in-admin | desktop | /vi/admin/promotions | 28 | 0 | 26 | 0 | 2794 | 580 | 896 | 0.0 | 0.0000 | 0 | 0 | 644104 |
| High | admin | logged-in-admin | desktop | /vi/admin/system-configs | 28 | 0 | 26 | 0 | 2753 | 540 | 1052 | 0.0 | 0.0000 | 0 | 0 | 655284 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/9efb4036-c003-4708-8e05-0835cb7574ee | 28 | 0 | 26 | 0 | 2749 | 560 | 972 | 0.0 | 0.0041 | 0 | 0 | 631499 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 2846 | 688 | 1100 | 0.0 | 0.0041 | 0 | 0 | 630574 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 2755 | 548 | 932 | 0.0 | 0.0041 | 0 | 0 | 630758 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 2827 | 596 | 1040 | 0.0 | 0.0041 | 0 | 0 | 630713 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 2840 | 560 | 1176 | 0.0 | 0.0041 | 0 | 0 | 632485 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 2841 | 572 | 1292 | 0.0 | 0.0041 | 0 | 0 | 632679 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 2754 | 548 | 1288 | 0.0 | 0.0041 | 0 | 0 | 632654 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/mfa | 28 | 0 | 26 | 0 | 2846 | 536 | 960 | 0.0 | 0.0037 | 0 | 0 | 631164 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/reader | 28 | 0 | 26 | 0 | 2715 | 532 | 1332 | 0.0 | 0.0037 | 0 | 0 | 631892 |
| High | reader-chat | logged-in-reader | desktop | /vi/chat | 28 | 0 | 26 | 0 | 2867 | 616 | 1064 | 0.0 | 0.0037 | 0 | 0 | 631150 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 2749 | 644 | 940 | 0.0 | 0.0037 | 0 | 0 | 631222 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 2759 | 548 | 944 | 0.0 | 0.0037 | 0 | 0 | 631444 |
| High | reader-chat | logged-in-reader | desktop | /vi/reader/apply | 28 | 0 | 26 | 0 | 2781 | 532 | 948 | 0.0 | 0.0037 | 0 | 0 | 631794 |
| High | reading | logged-in-reader | desktop | /vi/reading/history | 28 | 0 | 26 | 0 | 2757 | 540 | 1264 | 0.0 | 0.0037 | 0 | 0 | 632681 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/5c925eeb-da7f-4823-b37a-ed1b218d0992 | 28 | 0 | 26 | 0 | 2795 | 720 | 1120 | 0.0 | 0.0037 | 0 | 0 | 631928 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/b54d3013-9175-466a-a0e9-d1656fe1f90c | 28 | 0 | 26 | 0 | 2836 | 736 | 1104 | 0.0 | 0.0040 | 0 | 0 | 631964 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 2762 | 580 | 960 | 0.0 | 0.0037 | 0 | 0 | 630815 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 2896 | 732 | 1108 | 0.0 | 0.0037 | 0 | 0 | 630888 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 2846 | 552 | 988 | 0.0 | 0.0037 | 0 | 0 | 630857 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 2833 | 536 | 1368 | 0.0 | 0.0037 | 0 | 0 | 632466 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 2878 | 560 | 1224 | 0.0 | 0.0037 | 0 | 0 | 632642 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 2870 | 564 | 1104 | 0.0 | 0.0037 | 0 | 0 | 632465 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/mfa | 28 | 0 | 26 | 0 | 2799 | 516 | 860 | 0.0 | 0.0000 | 0 | 0 | 631032 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | 28 | 0 | 26 | 0 | 2752 | 468 | 1120 | 0.0 | 0.0760 | 0 | 0 | 630128 |
| High | reader-chat | logged-in-admin | mobile | /vi/chat | 28 | 0 | 26 | 0 | 2864 | 492 | 836 | 0.0 | 0.0000 | 0 | 0 | 630969 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 2754 | 464 | 824 | 0.0 | 0.0000 | 0 | 0 | 631812 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 28 | 0 | 26 | 0 | 2730 | 472 | 1132 | 0.0 | 0.0071 | 0 | 0 | 630436 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/notifications | 28 | 0 | 26 | 0 | 2741 | 448 | 808 | 0.0 | 0.0000 | 0 | 0 | 631449 |
| High | reader-chat | logged-in-admin | mobile | /vi/reader/apply | 28 | 0 | 26 | 0 | 2857 | 492 | 836 | 0.0 | 0.0000 | 0 | 0 | 632022 |
| High | reading | logged-in-admin | mobile | /vi/reading/history | 28 | 0 | 26 | 0 | 2737 | 464 | 880 | 4.0 | 0.0000 | 0 | 0 | 632262 |
| High | admin | logged-in-admin | mobile | /vi/admin/promotions | 28 | 0 | 26 | 0 | 2962 | 524 | 932 | 0.0 | 0.0000 | 0 | 0 | 644099 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 2846 | 480 | 820 | 0.0 | 0.0000 | 0 | 0 | 630779 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 2831 | 464 | 832 | 0.0 | 0.0000 | 0 | 0 | 632307 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 2807 | 452 | 800 | 0.0 | 0.0000 | 0 | 0 | 632346 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 2784 | 564 | 916 | 0.0 | 0.0000 | 0 | 0 | 632552 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/mfa | 28 | 0 | 26 | 0 | 2717 | 452 | 796 | 0.0 | 0.0000 | 0 | 0 | 631225 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/reader | 28 | 0 | 26 | 0 | 2870 | 476 | 844 | 0.0 | 0.0000 | 0 | 0 | 632074 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers | 28 | 0 | 26 | 0 | 2918 | 536 | 1176 | 0.0 | 0.0000 | 0 | 0 | 633527 |
| High | reader-chat | logged-in-reader | mobile | /vi/chat | 28 | 0 | 26 | 0 | 2835 | 472 | 808 | 0.0 | 0.0000 | 0 | 0 | 631336 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet | 28 | 0 | 26 | 0 | 2954 | 484 | 944 | 0.0 | 0.0000 | 0 | 0 | 633887 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 2849 | 476 | 824 | 0.0 | 0.0000 | 0 | 0 | 631329 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/notifications | 28 | 0 | 26 | 0 | 2825 | 464 | 808 | 0.0 | 0.0000 | 0 | 0 | 631841 |
| High | reader-chat | logged-in-reader | mobile | /vi/reader/apply | 28 | 0 | 26 | 0 | 2881 | 648 | 984 | 0.0 | 0.0000 | 0 | 0 | 631992 |
| High | reading | logged-in-reader | mobile | /vi/reading/history | 28 | 0 | 26 | 0 | 2763 | 456 | 808 | 0.0 | 0.0000 | 0 | 0 | 632559 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/0127a99b-82f6-44ce-8c33-e3a974758097 | 28 | 0 | 26 | 0 | 2850 | 544 | 900 | 0.0 | 0.0000 | 0 | 0 | 631746 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 2759 | 456 | 784 | 0.0 | 0.0000 | 0 | 0 | 630864 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 2823 | 468 | 800 | 0.0 | 0.0000 | 0 | 0 | 630818 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 2792 | 484 | 828 | 0.0 | 0.0000 | 0 | 0 | 630997 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 3009 | 572 | 1004 | 0.0 | 0.0000 | 0 | 0 | 632432 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 2869 | 468 | 868 | 0.0 | 0.0000 | 0 | 0 | 632793 |
| High | auth-public | logged-in-admin | desktop | /vi | 26 | 0 | 24 | 0 | 2776 | 648 | 1448 | 472.0 | 0.0035 | 0 | 0 | 537217 |
| High | auth-public | logged-in-reader | desktop | /vi | 26 | 0 | 24 | 0 | 2759 | 544 | 1172 | 385.0 | 0.0037 | 0 | 0 | 537037 |
| High | auth-public | logged-out | mobile | /vi | 26 | 0 | 24 | 0 | 3214 | 996 | 1352 | 0.0 | 0.0000 | 0 | 0 | 530425 |
| High | auth-public | logged-in-admin | mobile | /vi | 26 | 0 | 24 | 0 | 2777 | 516 | 884 | 0.0 | 0.0032 | 0 | 0 | 537021 |
| High | auth-public | logged-in-reader | mobile | /vi | 26 | 0 | 24 | 0 | 2759 | 488 | 856 | 0.0 | 0.0028 | 0 | 0 | 537102 |
| High | auth-public | logged-in-reader | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2954 | 776 | 1124 | 0.0 | 0.0018 | 0 | 0 | 525780 |
| Medium | auth-public | logged-out | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 2776 | 500 | 500 | 0.0 | 0.0000 | 0 | 0 | 525154 |
| Medium | auth-public | logged-out | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2716 | 500 | 500 | 0.0 | 0.0000 | 0 | 0 | 525348 |
| Medium | auth-public | logged-out | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2735 | 480 | 480 | 0.0 | 0.0000 | 0 | 0 | 525442 |
| Medium | auth-public | logged-in-admin | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 2682 | 568 | 932 | 0.0 | 0.0020 | 0 | 0 | 525616 |
| Medium | auth-public | logged-in-admin | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2714 | 564 | 892 | 0.0 | 0.0020 | 0 | 0 | 525683 |
| Medium | auth-public | logged-in-admin | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2775 | 548 | 880 | 0.0 | 0.0020 | 0 | 0 | 525744 |
| Medium | auth-public | logged-in-reader | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 2830 | 652 | 1044 | 0.0 | 0.0005 | 0 | 0 | 525654 |
| Medium | auth-public | logged-in-reader | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2726 | 484 | 840 | 0.0 | 0.0018 | 0 | 0 | 525757 |
| Medium | auth-public | logged-out | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 2765 | 480 | 504 | 0.0 | 0.0000 | 0 | 0 | 525307 |
| Medium | auth-public | logged-out | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2713 | 468 | 484 | 0.0 | 0.0000 | 0 | 0 | 525278 |
| Medium | auth-public | logged-out | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2717 | 476 | 804 | 0.0 | 0.0000 | 0 | 0 | 525289 |
| Medium | auth-public | logged-in-admin | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 2657 | 444 | 784 | 0.0 | 0.0032 | 0 | 0 | 525526 |
| Medium | auth-public | logged-in-admin | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2747 | 484 | 796 | 0.0 | 0.0032 | 0 | 0 | 525671 |
| Medium | auth-public | logged-in-admin | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2840 | 484 | 812 | 0.0 | 0.0032 | 0 | 0 | 525794 |
| Medium | auth-public | logged-in-reader | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 2768 | 520 | 852 | 0.0 | 0.0028 | 0 | 0 | 525649 |
| Medium | auth-public | logged-in-reader | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2754 | 500 | 836 | 0.0 | 0.0028 | 0 | 0 | 525675 |
| Medium | auth-public | logged-in-reader | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2761 | 484 | 816 | 0.0 | 0.0028 | 0 | 0 | 525807 |
| Medium | auth-public | logged-out | desktop | /vi/login | 24 | 0 | 22 | 0 | 2760 | 564 | 564 | 0.0 | 0.0000 | 0 | 0 | 511598 |
| Medium | auth-public | logged-out | desktop | /vi/register | 24 | 0 | 22 | 0 | 2801 | 680 | 680 | 0.0 | 0.0000 | 0 | 0 | 512339 |
| Medium | auth-public | logged-out | desktop | /vi/forgot-password | 24 | 0 | 22 | 0 | 2681 | 468 | 468 | 0.0 | 0.0000 | 0 | 0 | 511237 |
| Medium | auth-public | logged-out | desktop | /vi/reset-password | 24 | 0 | 22 | 0 | 2860 | 548 | 548 | 0.0 | 0.0000 | 0 | 0 | 511382 |
| Medium | auth-public | logged-out | desktop | /vi/verify-email | 24 | 0 | 22 | 0 | 2710 | 516 | 516 | 0.0 | 0.0000 | 0 | 0 | 511535 |
| Medium | auth-public | logged-out | mobile | /vi/login | 24 | 0 | 22 | 0 | 2704 | 476 | 476 | 0.0 | 0.0000 | 0 | 0 | 511706 |
| Medium | auth-public | logged-out | mobile | /vi/register | 24 | 0 | 22 | 0 | 2753 | 488 | 488 | 0.0 | 0.0000 | 0 | 0 | 512266 |
| Medium | auth-public | logged-out | mobile | /vi/forgot-password | 24 | 0 | 22 | 0 | 2683 | 464 | 464 | 0.0 | 0.0000 | 0 | 0 | 511303 |
| Medium | auth-public | logged-out | mobile | /vi/reset-password | 24 | 0 | 22 | 0 | 2753 | 476 | 476 | 0.0 | 0.0000 | 0 | 0 | 511403 |
| Medium | auth-public | logged-out | mobile | /vi/verify-email | 24 | 0 | 22 | 0 | 2734 | 584 | 584 | 0.0 | 0.0000 | 0 | 0 | 511559 |

## Major Issues Found

- Critical: chưa phát hiện page Critical theo benchmark hiện tại.
- High: 143 page(s) vượt ngưỡng >25 requests hoặc có request >800ms.
- Medium: 27 page(s) có request trong dải 400-800ms.
- Duplicate: chưa phát hiện duplicate request business đáng kể.
- Pending: chưa phát hiện pending request bất thường.

### Slow Requests

| Severity | Feature | Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile | GET | 200 | 1377 | 318 | html | https://www.tarotnow.xyz/vi/profile |
| Critical | auth-public | logged-out | desktop | /vi | GET | 200 | 1306 | 564 | static | https://www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |
| Critical | auth-public | logged-out | desktop | /vi | GET | 200 | 1304 | 556 | static | https://www.tarotnow.xyz/_next/static/chunks/0jpe042aa74zf.js |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1223 | 320 | third-party | https://img.tarotnow.xyz/light-god-50/08_The_Chariot_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1203 | 218 | third-party | https://img.tarotnow.xyz/light-god-50/07_The_Lovers_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | auth-public | logged-out | desktop | /vi | GET | 200 | 1173 | 395 | html | https://www.tarotnow.xyz/vi |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1094 | 114 | third-party | https://img.tarotnow.xyz/light-god-50/06_The_Hierophant_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | auth-public | logged-out | mobile | /vi | GET | 200 | 1055 | 676 | html | https://www.tarotnow.xyz/vi |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 1021 | 333 | html | https://www.tarotnow.xyz/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 987 | 246 | third-party | https://img.tarotnow.xyz/light-god-50/08_The_Chariot_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 977 | 323 | third-party | https://img.tarotnow.xyz/light-god-50/07_The_Lovers_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha | GET | 200 | 961 | 723 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Frare_title_lucky_star_50_20260416_165453.avif&w=256&q=75 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/reader/apply | GET | 200 | 933 | 329 | html | https://www.tarotnow.xyz/vi/reader/apply |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 909 | 106 | third-party | https://img.tarotnow.xyz/light-god-50/06_The_Hierophant_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 909 | 333 | html | https://www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |
| Critical | auth-public | logged-in-reader | desktop | /vi/legal/privacy | GET | 200 | 887 | 548 | html | https://www.tarotnow.xyz/vi/legal/privacy |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha | GET | 200 | 886 | 356 | html | https://www.tarotnow.xyz/vi/gacha |
| Critical | admin | logged-in-admin | mobile | /vi/admin/readings | GET | 200 | 883 | 327 | html | https://www.tarotnow.xyz/vi/admin/readings |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 881 | 106 | third-party | https://img.tarotnow.xyz/light-god-50/04_The_Empress_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | GET | 200 | 879 | 874 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Frare_title_lucky_star_50_20260416_165453.avif&w=48&q=75 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | GET | 200 | 879 | 415 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 874 | 114 | third-party | https://img.tarotnow.xyz/light-god-50/04_The_Empress_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | admin | logged-in-admin | mobile | /vi/admin/promotions | GET | 200 | 871 | 337 | html | https://www.tarotnow.xyz/vi/admin/promotions |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/withdraw | GET | 200 | 870 | 368 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 866 | 74 | third-party | https://img.tarotnow.xyz/light-god-50/07_The_Lovers_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 865 | 68 | third-party | https://img.tarotnow.xyz/light-god-50/03_The_High%20Priestess%20_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet | GET | 200 | 864 | 335 | html | https://www.tarotnow.xyz/vi/wallet |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 862 | 79 | third-party | https://img.tarotnow.xyz/light-god-50/05_The_Emperor_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | GET | 200 | 858 | 321 | html | https://www.tarotnow.xyz/vi/inventory |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile | GET | 200 | 854 | 321 | html | https://www.tarotnow.xyz/vi/profile |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/inventory | GET | 200 | 852 | 357 | html | https://www.tarotnow.xyz/vi/inventory |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet | GET | 200 | 850 | 308 | html | https://www.tarotnow.xyz/vi/wallet |
| Critical | reader-chat | logged-in-reader | desktop | /vi/readers | GET | 200 | 845 | 319 | html | https://www.tarotnow.xyz/vi/readers |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers | GET | 200 | 844 | 368 | html | https://www.tarotnow.xyz/vi/readers |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/withdraw | GET | 200 | 832 | 339 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | GET | 200 | 829 | 323 | html | https://www.tarotnow.xyz/vi/wallet |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/inventory | GET | 200 | 829 | 315 | html | https://www.tarotnow.xyz/vi/inventory |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 828 | 397 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/notifications | GET | 200 | 823 | 310 | html | https://www.tarotnow.xyz/vi/notifications |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha | GET | 200 | 820 | 324 | html | https://www.tarotnow.xyz/vi/gacha |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | GET | 200 | 816 | 316 | html | https://www.tarotnow.xyz/vi/wallet |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile | GET | 200 | 815 | 323 | html | https://www.tarotnow.xyz/vi/profile |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha | GET | 200 | 814 | 316 | html | https://www.tarotnow.xyz/vi/gacha |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet | GET | 200 | 812 | 305 | html | https://www.tarotnow.xyz/vi/wallet |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet | GET | 200 | 812 | 313 | html | https://www.tarotnow.xyz/vi/wallet |
| Critical | reading | logged-in-reader | desktop | /vi/reading | GET | 200 | 811 | 340 | html | https://www.tarotnow.xyz/vi/reading |
| Critical | admin | logged-in-admin | mobile | /vi/admin/users | GET | 200 | 808 | 317 | html | https://www.tarotnow.xyz/vi/admin/users |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/community | GET | 200 | 807 | 340 | html | https://www.tarotnow.xyz/vi/community |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha | GET | 200 | 806 | 332 | html | https://www.tarotnow.xyz/vi/gacha |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/notifications | GET | 200 | 804 | 325 | html | https://www.tarotnow.xyz/vi/notifications |
| Medium | reader-chat | logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 800 | 311 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| Medium | reader-chat | logged-in-admin | mobile | /vi/readers | GET | 200 | 800 | 320 | html | https://www.tarotnow.xyz/vi/readers |
| Medium | reading | logged-in-admin | mobile | /vi/reading | GET | 200 | 798 | 332 | html | https://www.tarotnow.xyz/vi/reading |
| Medium | reader-chat | logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 798 | 322 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| Medium | community-leaderboard-quest | logged-in-reader | mobile | /vi/gamification | GET | 200 | 798 | 327 | html | https://www.tarotnow.xyz/vi/gamification |
| Medium | reading | logged-in-admin | desktop | /vi/reading | GET | 200 | 796 | 321 | html | https://www.tarotnow.xyz/vi/reading |
| Medium | reader-chat | logged-in-admin | desktop | /vi/readers | GET | 200 | 796 | 324 | html | https://www.tarotnow.xyz/vi/readers |
| Medium | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | GET | 200 | 796 | 318 | html | https://www.tarotnow.xyz/vi/gamification |
| Medium | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | GET | 200 | 793 | 786 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Fpower_booster_50_20260416_165453.avif&w=48&q=75 |
| Medium | inventory-gacha-collection | logged-in-reader | desktop | /vi/inventory | GET | 200 | 793 | 322 | html | https://www.tarotnow.xyz/vi/inventory |
| Medium | auth-public | logged-out | desktop | /vi/reset-password | GET | 200 | 790 | 325 | html | https://www.tarotnow.xyz/vi/reset-password |
| Medium | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha/history | GET | 200 | 790 | 316 | html | https://www.tarotnow.xyz/vi/gacha/history |
| Medium | reader-chat | logged-in-reader | mobile | /vi/reader/apply | GET | 200 | 790 | 335 | html | https://www.tarotnow.xyz/vi/reader/apply |
| Medium | admin | logged-in-admin | desktop | /vi/admin | GET | 200 | 789 | 543 | static | https://www.tarotnow.xyz/_next/static/chunks/0w.yuzykfwjex.js |
| Medium | reading | logged-in-admin | mobile | /vi/reading/session/f481bbdc-f180-4075-b725-dfd8bd1ec4c3 | GET | 200 | 789 | 336 | html | https://www.tarotnow.xyz/vi/reading/session/f481bbdc-f180-4075-b725-dfd8bd1ec4c3 |
| Medium | reading | logged-in-reader | mobile | /vi/reading | GET | 200 | 789 | 310 | html | https://www.tarotnow.xyz/vi/reading |
| Medium | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | GET | 200 | 787 | 779 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Fexp_booster_50_20260416_165452.avif&w=48&q=75 |
| Medium | community-leaderboard-quest | logged-in-reader | desktop | /vi/gamification | GET | 200 | 786 | 321 | html | https://www.tarotnow.xyz/vi/gamification |
| Medium | admin | logged-in-admin | mobile | /vi/admin/reader-requests | GET | 200 | 786 | 345 | html | https://www.tarotnow.xyz/vi/admin/reader-requests |
| Medium | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | GET | 200 | 785 | 319 | html | https://www.tarotnow.xyz/vi/profile |
| Medium | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha/history | GET | 200 | 784 | 355 | html | https://www.tarotnow.xyz/vi/gacha/history |
| Medium | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha/history | GET | 200 | 780 | 307 | html | https://www.tarotnow.xyz/vi/gacha/history |
| Medium | admin | logged-in-admin | desktop | /vi/admin | GET | 200 | 779 | 537 | static | https://www.tarotnow.xyz/_next/static/chunks/0gyj3cptqnt.8.js |
| Medium | auth-public | logged-out | desktop | /vi | GET | 200 | 778 | 56 | telemetry | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Medium | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha/history | GET | 200 | 777 | 316 | html | https://www.tarotnow.xyz/vi/gacha/history |
| Medium | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | GET | 200 | 777 | 315 | html | https://www.tarotnow.xyz/vi/gamification |
| Medium | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | GET | 200 | 776 | 423 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| Medium | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | GET | 200 | 776 | 314 | html | https://www.tarotnow.xyz/vi/profile |
| Medium | reading | logged-in-reader | mobile | /vi/reading/session/b61cdc9c-52e7-4019-96b5-d60594bc7795 | GET | 200 | 775 | 324 | html | https://www.tarotnow.xyz/vi/reading/session/b61cdc9c-52e7-4019-96b5-d60594bc7795 |
| Medium | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | GET | 200 | 769 | 759 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Fdefense_booster_50_20260416_165452.avif&w=48&q=75 |

### Duplicate API / Request Candidates

| Severity | Feature | Scenario | Viewport | Route | Count | Request Key |
| --- | --- | --- | --- | --- | ---: | --- |
| - | - | - | - | - | - | - |

### Pending Requests

| Severity | Feature | Scenario | Viewport | Route | URL |
| --- | --- | --- | --- | --- | --- |
| - | - | - | - | - | - |

### Top Transfer Contributors

| Feature | Scenario | Viewport | Route | Category | Transfer bytes | Duration (ms) | Cache-Control | URL |
| --- | --- | --- | --- | --- | ---: | ---: | --- | --- |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | third-party | 325846 | 881 | - | img.tarotnow.xyz/light-god-50/04_The_Empress_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | third-party | 325839 | 220 | - | img.tarotnow.xyz/light-god-50/04_The_Empress_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | third-party | 325838 | 874 | - | img.tarotnow.xyz/light-god-50/04_The_Empress_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | third-party | 325811 | 250 | - | img.tarotnow.xyz/light-god-50/04_The_Empress_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | third-party | 259812 | 180 | - | img.tarotnow.xyz/light-god-50/01_The_Fool_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | third-party | 259789 | 552 | - | img.tarotnow.xyz/light-god-50/01_The_Fool_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | third-party | 259787 | 176 | - | img.tarotnow.xyz/light-god-50/01_The_Fool_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | third-party | 259780 | 711 | - | img.tarotnow.xyz/light-god-50/01_The_Fool_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | third-party | 231152 | 909 | - | img.tarotnow.xyz/light-god-50/06_The_Hierophant_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | third-party | 231149 | 1094 | - | img.tarotnow.xyz/light-god-50/06_The_Hierophant_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | third-party | 231117 | 760 | - | img.tarotnow.xyz/light-god-50/06_The_Hierophant_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | third-party | 231115 | 619 | - | img.tarotnow.xyz/light-god-50/06_The_Hierophant_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | third-party | 226228 | 150 | - | img.tarotnow.xyz/light-god-50/02_The_Magician_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | third-party | 226196 | 446 | - | img.tarotnow.xyz/light-god-50/02_The_Magician_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | third-party | 226130 | 551 | - | img.tarotnow.xyz/light-god-50/02_The_Magician_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | third-party | 226128 | 600 | - | img.tarotnow.xyz/light-god-50/02_The_Magician_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | third-party | 226127 | 667 | - | img.tarotnow.xyz/light-god-50/02_The_Magician_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | third-party | 226126 | 185 | - | img.tarotnow.xyz/light-god-50/02_The_Magician_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | third-party | 226125 | 671 | - | img.tarotnow.xyz/light-god-50/02_The_Magician_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | third-party | 226119 | 323 | - | img.tarotnow.xyz/light-god-50/02_The_Magician_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | third-party | 211489 | 866 | - | img.tarotnow.xyz/light-god-50/07_The_Lovers_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | third-party | 211476 | 1203 | - | img.tarotnow.xyz/light-god-50/07_The_Lovers_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | third-party | 211474 | 626 | - | img.tarotnow.xyz/light-god-50/07_The_Lovers_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | third-party | 211454 | 977 | - | img.tarotnow.xyz/light-god-50/07_The_Lovers_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | third-party | 186966 | 675 | - | img.tarotnow.xyz/light-god-50/05_The_Emperor_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | third-party | 186963 | 862 | - | img.tarotnow.xyz/light-god-50/05_The_Emperor_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | third-party | 186857 | 163 | - | img.tarotnow.xyz/light-god-50/05_The_Emperor_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | third-party | 186831 | 130 | - | img.tarotnow.xyz/light-god-50/05_The_Emperor_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | third-party | 173420 | 155 | - | img.tarotnow.xyz/light-god-50/03_The_High%20Priestess%20_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | third-party | 173417 | 865 | - | img.tarotnow.xyz/light-god-50/03_The_High%20Priestess%20_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | third-party | 173311 | 331 | - | img.tarotnow.xyz/light-god-50/03_The_High%20Priestess%20_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | third-party | 173297 | 695 | - | img.tarotnow.xyz/light-god-50/03_The_High%20Priestess%20_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | third-party | 173286 | 286 | - | img.tarotnow.xyz/light-god-50/03_The_High%20Priestess%20_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | third-party | 173265 | 157 | - | img.tarotnow.xyz/light-god-50/03_The_High%20Priestess%20_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | third-party | 173262 | 127 | - | img.tarotnow.xyz/light-god-50/03_The_High%20Priestess%20_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | third-party | 173260 | 145 | - | img.tarotnow.xyz/light-god-50/03_The_High%20Priestess%20_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | third-party | 155855 | 1223 | - | img.tarotnow.xyz/light-god-50/08_The_Chariot_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | third-party | 155844 | 987 | - | img.tarotnow.xyz/light-god-50/08_The_Chariot_50_20260325_181351.avif?iv=81a3d9698977fda2&variant=thumb |
| inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha | static | 89038 | 127 | - | www.tarotnow.xyz/_next/static/chunks/0to-xfrh5ita1.js |
| inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha | static | 89036 | 172 | - | www.tarotnow.xyz/_next/static/chunks/0to-xfrh5ita1.js |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha | static | 89035 | 115 | - | www.tarotnow.xyz/_next/static/chunks/0to-xfrh5ita1.js |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha/history | static | 89035 | 101 | - | www.tarotnow.xyz/_next/static/chunks/0to-xfrh5ita1.js |
| inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha/history | static | 89033 | 167 | - | www.tarotnow.xyz/_next/static/chunks/0to-xfrh5ita1.js |
| inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha | static | 89033 | 122 | - | www.tarotnow.xyz/_next/static/chunks/0to-xfrh5ita1.js |
| inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha/history | static | 89033 | 125 | - | www.tarotnow.xyz/_next/static/chunks/0to-xfrh5ita1.js |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha/history | static | 89028 | 126 | - | www.tarotnow.xyz/_next/static/chunks/0to-xfrh5ita1.js |
| reading | logged-in-admin | desktop | /vi/reading | static | 81457 | 335 | - | www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |
| reading | logged-in-admin | mobile | /vi/reading | static | 81456 | 343 | - | www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |
| auth-public | logged-in-reader | mobile | /vi | static | 81449 | 329 | - | www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | static | 81445 | 417 | - | www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |
| profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet | static | 81442 | 352 | - | www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |
| auth-public | logged-in-reader | desktop | /vi | static | 81442 | 356 | - | www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |
| reader-chat | logged-in-admin | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | static | 81442 | 321 | - | www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |
| inventory-gacha-collection | logged-in-reader | mobile | /vi/inventory | static | 81442 | 459 | - | www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |
| reader-chat | logged-in-admin | mobile | /vi/chat | static | 81441 | 371 | - | www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |
| admin | logged-in-admin | mobile | /vi/admin | static | 81441 | 337 | - | www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |
| reader-chat | logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | static | 81441 | 381 | - | www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |
| inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha/history | static | 81440 | 331 | - | www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |
| inventory-gacha-collection | logged-in-reader | desktop | /vi/inventory | static | 81440 | 402 | - | www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |
| reader-chat | logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | static | 81440 | 314 | - | www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |
| auth-public | logged-in-admin | mobile | /vi/legal/privacy | static | 81440 | 256 | - | www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |
| reading | logged-in-admin | mobile | /vi/reading/session/6f8210b4-a511-4ee4-99a7-e0730efe4828 | static | 81440 | 328 | - | www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |
| auth-public | logged-out | desktop | /vi/register | static | 81439 | 339 | - | www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |
| reading | logged-in-admin | desktop | /vi/reading/session/dbbf695c-841c-43a9-87a6-6f163bb469cd | static | 81439 | 333 | - | www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |
| reader-chat | logged-in-reader | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | static | 81439 | 386 | - | www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |
| profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | static | 81439 | 307 | - | www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |
| auth-public | logged-in-admin | mobile | /vi/legal/ai-disclaimer | static | 81439 | 378 | - | www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |
| auth-public | logged-in-reader | desktop | /vi/legal/tos | static | 81438 | 372 | - | www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |
| reading | logged-in-reader | desktop | /vi/reading/session/5c925eeb-da7f-4823-b37a-ed1b218d0992 | static | 81438 | 365 | - | www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |
| admin | logged-in-admin | mobile | /vi/admin/system-configs | static | 81438 | 312 | - | www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |
| profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | static | 81437 | 405 | - | www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |
| reader-chat | logged-in-admin | desktop | /vi/reader/apply | static | 81437 | 519 | - | www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |
| inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha | static | 81437 | 458 | - | www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |
| auth-public | logged-in-reader | desktop | /vi/legal/privacy | static | 81437 | 294 | - | www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |
| admin | logged-in-admin | mobile | /vi/admin/disputes | static | 81437 | 369 | - | www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |
| inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha | static | 81437 | 451 | - | www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |
| inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha/history | static | 81437 | 294 | - | www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |
| profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet | static | 81437 | 489 | - | www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |
| auth-public | logged-in-reader | mobile | /vi/legal/tos | static | 81437 | 292 | - | www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |
| auth-public | logged-out | desktop | /vi/reset-password | static | 81436 | 437 | - | www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |

### Cacheability Issues

| Feature | Scenario | Viewport | Route | Category | Issue | Transfer bytes | Cache-Control | URL |
| --- | --- | --- | --- | --- | --- | ---: | --- | --- |
| - | - | - | - | - | - | - | - | - |

### Waterfall Sample

| Feature | Scenario | Viewport | Route | Start (ms) | End (ms) | Duration (ms) | Type | Category | URL |
| --- | --- | --- | --- | ---: | ---: | ---: | --- | --- | --- |
| - | - | - | - | - | - | - | - | - | - |

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
