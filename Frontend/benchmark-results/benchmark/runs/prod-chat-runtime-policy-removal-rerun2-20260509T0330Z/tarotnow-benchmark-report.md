# TarotNow Navigation Benchmark Report

- Generated at (UTC): 2026-05-08T20:32:14.963Z
- Base URL: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Thresholds: >35 requests = Critical, >25 = High, request >800ms = High, >400ms = Medium
- Critical pages (request count): 0
- High pages (request count): 143
- High slow requests: 121
- Medium slow requests: 438

## Scenario Summary
| Scenario | Viewport | Pages Benchmarked | Avg Navigation (ms) | Total Requests | Pending Requests | Document Reloads | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 2833 | 224 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 43 | 2950 | 1235 | 0 | 0 | 11 | 0 | yes |
| logged-in-reader | desktop | 33 | 3077 | 946 | 0 | 0 | 15 | 0 | yes |
| logged-out | mobile | 9 | 2828 | 224 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 43 | 3070 | 1254 | 0 | 0 | 17 | 0 | yes |
| logged-in-reader | mobile | 33 | 3318 | 959 | 0 | 0 | 14 | 0 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.4 | 2860 | 0 |
| logged-in-admin | desktop | chat | 1 | 28.0 | 2731 | 0 |
| logged-in-admin | desktop | collection | 1 | 29.0 | 6606 | 0 |
| logged-in-admin | desktop | community | 1 | 30.0 | 3668 | 0 |
| logged-in-admin | desktop | gacha | 2 | 31.0 | 2914 | 0 |
| logged-in-admin | desktop | gamification | 1 | 29.0 | 2928 | 0 |
| logged-in-admin | desktop | home | 1 | 26.0 | 2743 | 0 |
| logged-in-admin | desktop | inventory | 1 | 31.0 | 2968 | 0 |
| logged-in-admin | desktop | leaderboard | 1 | 30.0 | 2872 | 0 |
| logged-in-admin | desktop | legal | 3 | 25.0 | 2691 | 0 |
| logged-in-admin | desktop | notifications | 1 | 28.0 | 2786 | 0 |
| logged-in-admin | desktop | profile | 3 | 29.0 | 2811 | 0 |
| logged-in-admin | desktop | reader | 1 | 29.0 | 2961 | 0 |
| logged-in-admin | desktop | readers | 7 | 28.1 | 2830 | 0 |
| logged-in-admin | desktop | reading | 5 | 29.4 | 2868 | 0 |
| logged-in-admin | desktop | wallet | 4 | 28.3 | 2871 | 0 |
| logged-in-admin | mobile | admin | 10 | 29.5 | 2936 | 0 |
| logged-in-admin | mobile | chat | 1 | 28.0 | 2817 | 0 |
| logged-in-admin | mobile | collection | 1 | 29.0 | 6267 | 0 |
| logged-in-admin | mobile | community | 1 | 35.0 | 3734 | 0 |
| logged-in-admin | mobile | gacha | 2 | 31.0 | 2850 | 0 |
| logged-in-admin | mobile | gamification | 1 | 30.0 | 2912 | 0 |
| logged-in-admin | mobile | home | 1 | 26.0 | 2752 | 0 |
| logged-in-admin | mobile | inventory | 1 | 32.0 | 2853 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 31.0 | 2887 | 0 |
| logged-in-admin | mobile | legal | 3 | 25.7 | 3556 | 0 |
| logged-in-admin | mobile | notifications | 1 | 28.0 | 2755 | 0 |
| logged-in-admin | mobile | profile | 3 | 29.0 | 2969 | 0 |
| logged-in-admin | mobile | reader | 1 | 28.0 | 3401 | 0 |
| logged-in-admin | mobile | readers | 7 | 28.6 | 2907 | 0 |
| logged-in-admin | mobile | reading | 5 | 30.6 | 3076 | 0 |
| logged-in-admin | mobile | wallet | 4 | 28.3 | 2819 | 0 |
| logged-in-reader | desktop | chat | 1 | 28.0 | 2726 | 0 |
| logged-in-reader | desktop | collection | 1 | 29.0 | 6845 | 0 |
| logged-in-reader | desktop | community | 1 | 30.0 | 3782 | 0 |
| logged-in-reader | desktop | gacha | 2 | 32.0 | 2915 | 0 |
| logged-in-reader | desktop | gamification | 1 | 29.0 | 2922 | 0 |
| logged-in-reader | desktop | home | 1 | 26.0 | 2759 | 0 |
| logged-in-reader | desktop | inventory | 1 | 30.0 | 2901 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 30.0 | 2907 | 0 |
| logged-in-reader | desktop | legal | 3 | 25.0 | 3050 | 0 |
| logged-in-reader | desktop | notifications | 1 | 28.0 | 2899 | 0 |
| logged-in-reader | desktop | profile | 3 | 29.0 | 2886 | 0 |
| logged-in-reader | desktop | reader | 1 | 29.0 | 2991 | 0 |
| logged-in-reader | desktop | readers | 7 | 28.3 | 2831 | 0 |
| logged-in-reader | desktop | reading | 5 | 29.6 | 3070 | 0 |
| logged-in-reader | desktop | wallet | 4 | 28.8 | 3000 | 0 |
| logged-in-reader | mobile | chat | 1 | 28.0 | 2938 | 0 |
| logged-in-reader | mobile | collection | 1 | 29.0 | 6291 | 0 |
| logged-in-reader | mobile | community | 1 | 30.0 | 3934 | 0 |
| logged-in-reader | mobile | gacha | 2 | 30.5 | 2940 | 0 |
| logged-in-reader | mobile | gamification | 1 | 30.0 | 2970 | 0 |
| logged-in-reader | mobile | home | 1 | 34.0 | 3427 | 0 |
| logged-in-reader | mobile | inventory | 1 | 31.0 | 2997 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 30.0 | 3042 | 0 |
| logged-in-reader | mobile | legal | 3 | 25.0 | 3489 | 0 |
| logged-in-reader | mobile | notifications | 1 | 29.0 | 3829 | 0 |
| logged-in-reader | mobile | profile | 3 | 29.0 | 2944 | 0 |
| logged-in-reader | mobile | reader | 1 | 28.0 | 4095 | 0 |
| logged-in-reader | mobile | readers | 7 | 28.0 | 2855 | 0 |
| logged-in-reader | mobile | reading | 5 | 31.0 | 3348 | 0 |
| logged-in-reader | mobile | wallet | 4 | 29.0 | 3515 | 0 |
| logged-out | desktop | auth | 5 | 24.0 | 2754 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 3584 | 0 |
| logged-out | desktop | legal | 3 | 25.0 | 2714 | 0 |
| logged-out | mobile | auth | 5 | 24.0 | 2756 | 0 |
| logged-out | mobile | home | 1 | 29.0 | 3364 | 0 |
| logged-out | mobile | legal | 3 | 25.0 | 2769 | 0 |

## Per-Page Metrics
| Scenario | Viewport | Feature | Route | Requests | Interaction Requests | Severity | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Collection Img Requests | Collection Img 400-800ms | Collection Img >800ms | Collection Img First Load | Collection Img Reopen | Collection Img Cache Hits | Navigate (ms) | DOMContentLoaded (ms) | Load (ms) | FCP (ms) | LCP (ms) | CLS | TBT (ms) | Transfer Bytes |
| --- | --- | --- | --- | ---: | ---: | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-out | desktop | auth-public | /vi | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3584 | 1415 | 1443 | 1412 | 1412 | 0.0000 | 387.0 | 600874 |
| logged-out | desktop | auth-public | /vi/login | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2754 | 708 | 742 | 668 | 668 | 0.0000 | 0.0 | 511653 |
| logged-out | desktop | auth-public | /vi/register | 24 | 2 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2724 | 704 | 704 | 580 | 580 | 0.0000 | 0.0 | 512196 |
| logged-out | desktop | auth-public | /vi/forgot-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2711 | 700 | 700 | 500 | 500 | 0.0000 | 0.0 | 511273 |
| logged-out | desktop | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2725 | 708 | 708 | 552 | 552 | 0.0000 | 0.0 | 511410 |
| logged-out | desktop | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2858 | 813 | 847 | 732 | 732 | 0.0000 | 0.0 | 511531 |
| logged-out | desktop | auth-public | /vi/legal/tos | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2697 | 681 | 681 | 572 | 896 | 0.0000 | 0.0 | 525275 |
| logged-out | desktop | auth-public | /vi/legal/privacy | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2702 | 691 | 691 | 476 | 804 | 0.0000 | 0.0 | 525288 |
| logged-out | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2744 | 729 | 729 | 608 | 608 | 0.0000 | 0.0 | 525284 |
| logged-in-admin | desktop | auth-public | /vi | 26 | 12 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2743 | 726 | 727 | 712 | 1384 | 0.0039 | 333.0 | 536864 |
| logged-in-admin | desktop | reading | /vi/reading | 29 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2935 | 889 | 922 | 540 | 992 | 0.0041 | 0.0 | 642170 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | 31 | 8 | high | 0 | 0 | 1 | 0 | 5 | 1 | 0 | 5 | 0 | 0 | 2968 | 955 | 955 | 600 | 1284 | 0.0041 | 0.0 | 644640 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | 32 | 9 | high | 0 | 0 | 2 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2914 | 890 | 891 | 512 | 1236 | 0.0041 | 0.0 | 726067 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | 30 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2914 | 849 | 887 | 580 | 1228 | 0.0041 | 0.0 | 725635 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 29 | 27 | high | 0 | 0 | 0 | 0 | 13 | 3 | 5 | 12 | 1 | 0 | 6606 | 704 | 720 | 532 | 532 | 0.0042 | 0.0 | 642680 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2978 | 852 | 965 | 552 | 1216 | 0.0489 | 0.0 | 635553 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2733 | 704 | 724 | 508 | 940 | 0.0041 | 0.0 | 631025 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | 38 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2722 | 908 | 1004 | 584 | 1216 | 0.0489 | 0.0 | 630329 |
| logged-in-admin | desktop | reader-chat | /vi/readers | 29 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2873 | 836 | 836 | 528 | 1172 | 0.0041 | 0.0 | 634469 |
| logged-in-admin | desktop | reader-chat | /vi/chat | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2731 | 698 | 717 | 560 | 988 | 0.0041 | 0.0 | 631226 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | 30 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2872 | 850 | 851 | 632 | 1084 | 0.0041 | 0.0 | 650815 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 30 | 9 | high | 0 | 0 | 0 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 3668 | 768 | 789 | 552 | 1768 | 0.0041 | 0.0 | 642640 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | 29 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2928 | 884 | 884 | 568 | 1044 | 0.0279 | 6.0 | 642377 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 29 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2903 | 854 | 854 | 568 | 1088 | 0.0041 | 0.0 | 634589 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2747 | 707 | 727 | 536 | 880 | 0.0041 | 0.0 | 630961 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2975 | 885 | 948 | 628 | 1276 | 0.0041 | 0.0 | 631790 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 38 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2857 | 822 | 880 | 576 | 1116 | 0.0041 | 0.0 | 630483 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2786 | 773 | 773 | 556 | 1032 | 0.0041 | 0.0 | 631793 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | 29 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2961 | 859 | 887 | 548 | 1012 | 0.0041 | 0.0 | 632696 |
| logged-in-admin | desktop | reading | /vi/reading/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2774 | 742 | 760 | 532 | 1156 | 0.0041 | 0.0 | 632290 |
| logged-in-admin | desktop | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2685 | 673 | 673 | 536 | 844 | 0.0020 | 0.0 | 525600 |
| logged-in-admin | desktop | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2698 | 679 | 679 | 532 | 936 | 0.0020 | 0.0 | 525613 |
| logged-in-admin | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2689 | 670 | 670 | 544 | 856 | 0.0020 | 0.0 | 525659 |
| logged-in-admin | desktop | admin | /vi/admin | 30 | 2 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2882 | 711 | 857 | 516 | 916 | 0.0000 | 0.0 | 648368 |
| logged-in-admin | desktop | admin | /vi/admin/deposits | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2954 | 877 | 902 | 644 | 1108 | 0.0000 | 0.0 | 646965 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2875 | 837 | 858 | 580 | 992 | 0.0000 | 0.0 | 645394 |
| logged-in-admin | desktop | admin | /vi/admin/gamification | 32 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2997 | 805 | 956 | 532 | 908 | 0.0022 | 0.0 | 697982 |
| logged-in-admin | desktop | admin | /vi/admin/promotions | 28 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2788 | 745 | 769 | 616 | 940 | 0.0000 | 0.0 | 644087 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2812 | 760 | 782 | 532 | 868 | 0.0000 | 0.0 | 646056 |
| logged-in-admin | desktop | admin | /vi/admin/readings | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2889 | 842 | 850 | 560 | 1120 | 0.0000 | 0.0 | 648266 |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2720 | 687 | 708 | 564 | 1060 | 0.0000 | 0.0 | 655250 |
| logged-in-admin | desktop | admin | /vi/admin/users | 31 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2899 | 840 | 885 | 592 | 1216 | 0.0000 | 0.0 | 651088 |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2785 | 742 | 761 | 536 | 900 | 0.0000 | 0.0 | 645487 |
| logged-in-admin | desktop | reading | /vi/reading/session/275c6df1-5a67-445b-a9db-b1d5ba316b23 | 34 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3098 | 918 | 1081 | 568 | 1036 | 0.0041 | 0.0 | 724930 |
| logged-in-admin | desktop | reading | /vi/reading/session/698106b6-6bdb-4988-93e6-87034cd28eda | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2760 | 734 | 745 | 512 | 944 | 0.0041 | 0.0 | 631929 |
| logged-in-admin | desktop | reading | /vi/reading/session/1a03a485-ec7d-4912-9cde-416b5bae69f3 | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2774 | 736 | 758 | 572 | 968 | 0.0041 | 0.0 | 631926 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2860 | 782 | 841 | 524 | 936 | 0.0041 | 0.0 | 630805 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2903 | 811 | 871 | 540 | 952 | 0.0041 | 0.0 | 630680 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2732 | 699 | 717 | 548 | 980 | 0.0041 | 0.0 | 630575 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2802 | 739 | 785 | 656 | 1352 | 0.0041 | 0.0 | 632450 |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2777 | 738 | 756 | 580 | 1228 | 0.0041 | 0.0 | 632612 |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2862 | 753 | 787 | 548 | 1228 | 0.0041 | 0.0 | 632421 |
| logged-in-reader | desktop | auth-public | /vi | 26 | 12 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2759 | 726 | 747 | 556 | 1196 | 0.0033 | 275.0 | 537014 |
| logged-in-reader | desktop | reading | /vi/reading | 30 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2953 | 875 | 937 | 524 | 1028 | 0.0039 | 0.0 | 643146 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | 30 | 9 | high | 0 | 0 | 0 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2901 | 837 | 845 | 524 | 1196 | 0.0039 | 0.0 | 644268 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | 32 | 9 | high | 0 | 0 | 2 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2916 | 893 | 893 | 544 | 1232 | 0.0039 | 0.0 | 726257 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | 32 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2914 | 887 | 888 | 580 | 1392 | 0.0039 | 0.0 | 725383 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 29 | 25 | high | 0 | 0 | 0 | 0 | 6 | 4 | 0 | 6 | 0 | 0 | 6845 | 750 | 785 | 544 | 544 | 0.0040 | 12.0 | 641734 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | 31 | 6 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2954 | 873 | 940 | 560 | 1268 | 0.0726 | 0.0 | 635521 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2764 | 735 | 752 | 536 | 924 | 0.0039 | 0.0 | 631133 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2939 | 848 | 880 | 520 | 1368 | 0.0039 | 0.0 | 631910 |
| logged-in-reader | desktop | reader-chat | /vi/readers | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2930 | 865 | 913 | 592 | 984 | 0.0039 | 0.0 | 633284 |
| logged-in-reader | desktop | reader-chat | /vi/chat | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2726 | 687 | 711 | 552 | 976 | 0.0039 | 0.0 | 631363 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | 30 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2907 | 851 | 852 | 552 | 992 | 0.0039 | 0.0 | 650451 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 30 | 9 | high | 0 | 0 | 0 | 0 | 1 | 0 | 1 | 1 | 0 | 0 | 3782 | 834 | 858 | 616 | 1856 | 0.0039 | 0.0 | 642787 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | 29 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2922 | 854 | 854 | 520 | 996 | 0.0277 | 0.0 | 641953 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2951 | 874 | 883 | 592 | 1160 | 0.0039 | 0.0 | 633541 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2967 | 890 | 932 | 580 | 1040 | 0.0039 | 0.0 | 631816 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3102 | 1009 | 1078 | 640 | 1168 | 0.0039 | 0.0 | 631966 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 29 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2981 | 883 | 957 | 564 | 1204 | 0.0095 | 0.0 | 633084 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2899 | 814 | 877 | 584 | 1012 | 0.0040 | 0.0 | 631748 |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | 29 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2991 | 911 | 945 | 620 | 1032 | 0.0039 | 0.0 | 632602 |
| logged-in-reader | desktop | reading | /vi/reading/history | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3898 | 1362 | 1414 | 1204 | 1204 | 0.0039 | 0.0 | 632436 |
| logged-in-reader | desktop | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3425 | 900 | 900 | 744 | 1056 | 0.0019 | 0.0 | 525539 |
| logged-in-reader | desktop | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2970 | 808 | 808 | 668 | 1152 | 0.0019 | 0.0 | 525633 |
| logged-in-reader | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2756 | 733 | 733 | 572 | 960 | 0.0019 | 0.0 | 525624 |
| logged-in-reader | desktop | reading | /vi/reading/session/84843a3c-745f-4205-8056-030a40207dde | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2746 | 708 | 730 | 520 | 956 | 0.0039 | 0.0 | 631781 |
| logged-in-reader | desktop | reading | /vi/reading/session/b27bf782-c302-4ae8-ab53-0f71b70a5c67 | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2759 | 741 | 741 | 556 | 904 | 0.0039 | 0.0 | 631616 |
| logged-in-reader | desktop | reading | /vi/reading/session/bfb8919e-c739-44f7-8934-63cd5dbeb9e4 | 34 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2996 | 813 | 971 | 536 | 924 | 0.0039 | 0.0 | 724683 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2921 | 852 | 883 | 580 | 1012 | 0.0039 | 0.0 | 631437 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2927 | 843 | 877 | 532 | 988 | 0.0039 | 0.0 | 631292 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2716 | 678 | 700 | 524 | 876 | 0.0039 | 0.0 | 630790 |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2772 | 727 | 751 | 548 | 1232 | 0.0039 | 0.0 | 632435 |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2808 | 764 | 777 | 576 | 1120 | 0.0039 | 0.0 | 632596 |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2742 | 701 | 724 | 560 | 1232 | 0.0039 | 0.0 | 632627 |
| logged-out | mobile | auth-public | /vi | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3364 | 1313 | 1313 | 964 | 964 | 0.0000 | 0.0 | 602715 |
| logged-out | mobile | auth-public | /vi/login | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2691 | 660 | 681 | 500 | 500 | 0.0000 | 0.0 | 511760 |
| logged-out | mobile | auth-public | /vi/register | 24 | 2 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2785 | 761 | 761 | 596 | 596 | 0.0000 | 0.0 | 512168 |
| logged-out | mobile | auth-public | /vi/forgot-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2692 | 676 | 676 | 472 | 472 | 0.0000 | 0.0 | 511351 |
| logged-out | mobile | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2703 | 684 | 684 | 468 | 468 | 0.0000 | 0.0 | 511371 |
| logged-out | mobile | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2909 | 892 | 893 | 684 | 684 | 0.0000 | 0.0 | 511499 |
| logged-out | mobile | auth-public | /vi/legal/tos | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2725 | 708 | 708 | 544 | 872 | 0.0000 | 0.0 | 525177 |
| logged-out | mobile | auth-public | /vi/legal/privacy | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2814 | 781 | 794 | 604 | 908 | 0.0000 | 0.0 | 525244 |
| logged-out | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2769 | 755 | 755 | 600 | 932 | 0.0000 | 0.0 | 525408 |
| logged-in-admin | mobile | auth-public | /vi | 26 | 12 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2752 | 712 | 726 | 532 | 904 | 0.0032 | 0.0 | 537041 |
| logged-in-admin | mobile | reading | /vi/reading | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2931 | 896 | 905 | 520 | 888 | 0.0000 | 0.0 | 641107 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | 32 | 8 | high | 0 | 0 | 2 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2853 | 830 | 830 | 452 | 1080 | 0.0000 | 0.0 | 645811 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | 32 | 9 | high | 0 | 0 | 2 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2845 | 826 | 826 | 436 | 1084 | 0.0000 | 0.0 | 725960 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | 30 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2855 | 830 | 830 | 452 | 836 | 0.0000 | 0.0 | 725653 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 29 | 26 | high | 0 | 0 | 0 | 0 | 13 | 3 | 5 | 12 | 1 | 0 | 6267 | 870 | 919 | 512 | 896 | 0.0000 | 47.0 | 642717 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3027 | 838 | 1007 | 456 | 1108 | 0.0689 | 0.0 | 634315 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3059 | 947 | 948 | 484 | 908 | 0.0000 | 0.0 | 631628 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | 38 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2822 | 1019 | 1145 | 732 | 1080 | 0.0760 | 0.0 | 630117 |
| logged-in-admin | mobile | reader-chat | /vi/readers | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2847 | 822 | 822 | 476 | 1108 | 0.0000 | 0.0 | 633267 |
| logged-in-admin | mobile | reader-chat | /vi/chat | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2817 | 739 | 797 | 440 | 792 | 0.0000 | 0.0 | 631475 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | 31 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2887 | 858 | 858 | 460 | 1116 | 0.0000 | 0.0 | 651025 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 35 | 3 | high | 0 | 0 | 1 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 3734 | 880 | 970 | 480 | 1672 | 0.0051 | 0.0 | 776467 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 30 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2912 | 870 | 870 | 448 | 832 | 0.0000 | 0.0 | 642788 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2872 | 844 | 844 | 448 | 1096 | 0.0000 | 0.0 | 634452 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2813 | 739 | 796 | 448 | 796 | 0.0000 | 0.0 | 631176 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2786 | 759 | 767 | 496 | 844 | 0.0000 | 0.0 | 631887 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 37 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2804 | 924 | 925 | 492 | 1152 | 0.0071 | 0.0 | 630591 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2755 | 728 | 736 | 476 | 820 | 0.0000 | 0.0 | 631626 |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3401 | 921 | 929 | 512 | 864 | 0.0071 | 0.0 | 632017 |
| logged-in-admin | mobile | reading | /vi/reading/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3587 | 752 | 812 | 656 | 1016 | 0.0000 | 0.0 | 632278 |
| logged-in-admin | mobile | auth-public | /vi/legal/tos | 27 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3655 | 1218 | 1218 | 624 | 940 | 0.0032 | 0.0 | 527310 |
| logged-in-admin | mobile | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3305 | 919 | 930 | 720 | 720 | 0.0032 | 0.0 | 525506 |
| logged-in-admin | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3708 | 994 | 1040 | 688 | 1012 | 0.0032 | 0.0 | 525843 |
| logged-in-admin | mobile | admin | /vi/admin | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3037 | 923 | 954 | 620 | 948 | 0.0000 | 0.0 | 647033 |
| logged-in-admin | mobile | admin | /vi/admin/deposits | 30 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2998 | 871 | 930 | 504 | 1176 | 0.0000 | 0.0 | 648759 |
| logged-in-admin | mobile | admin | /vi/admin/disputes | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2916 | 788 | 798 | 488 | 800 | 0.0000 | 0.0 | 645289 |
| logged-in-admin | mobile | admin | /vi/admin/gamification | 32 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3031 | 853 | 961 | 440 | 816 | 0.0000 | 0.0 | 697990 |
| logged-in-admin | mobile | admin | /vi/admin/promotions | 28 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2828 | 788 | 803 | 484 | 804 | 0.0000 | 0.0 | 644094 |
| logged-in-admin | mobile | admin | /vi/admin/reader-requests | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2844 | 804 | 817 | 496 | 820 | 0.0000 | 0.0 | 645835 |
| logged-in-admin | mobile | admin | /vi/admin/readings | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2894 | 828 | 830 | 484 | 856 | 0.0000 | 0.0 | 648260 |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | 30 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2973 | 829 | 952 | 508 | 896 | 0.0000 | 0.0 | 688605 |
| logged-in-admin | mobile | admin | /vi/admin/users | 30 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2947 | 872 | 872 | 500 | 1140 | 0.0000 | 0.0 | 649362 |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2894 | 820 | 823 | 484 | 804 | 0.0000 | 0.0 | 645598 |
| logged-in-admin | mobile | reading | /vi/reading/session/ad085b60-8b51-43a6-9018-4217d2c3914d | 33 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2972 | 798 | 946 | 452 | 804 | 0.0000 | 0.0 | 692961 |
| logged-in-admin | mobile | reading | /vi/reading/session/09ef272e-4364-4d56-82ba-498ed1c6ead7 | 33 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2977 | 807 | 957 | 468 | 804 | 0.0000 | 0.0 | 692978 |
| logged-in-admin | mobile | reading | /vi/reading/session/56c81cad-8f33-4a8b-aae2-ea3499e78a3a | 31 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2911 | 844 | 888 | 472 | 820 | 0.0000 | 0.0 | 681154 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2757 | 725 | 734 | 476 | 812 | 0.0000 | 0.0 | 630531 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2893 | 838 | 840 | 472 | 836 | 0.0000 | 0.0 | 631383 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2933 | 806 | 821 | 536 | 876 | 0.0000 | 0.0 | 630671 |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2750 | 707 | 708 | 452 | 804 | 0.0000 | 0.0 | 632104 |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3004 | 876 | 884 | 452 | 824 | 0.0000 | 0.0 | 632941 |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 30 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3168 | 861 | 892 | 488 | 872 | 0.0000 | 0.0 | 634272 |
| logged-in-reader | mobile | auth-public | /vi | 34 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3427 | 843 | 1114 | 912 | 912 | 0.0032 | 2.0 | 611090 |
| logged-in-reader | mobile | reading | /vi/reading | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2949 | 905 | 919 | 488 | 848 | 0.0000 | 0.0 | 641887 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | 31 | 8 | high | 0 | 0 | 1 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2997 | 940 | 940 | 488 | 1156 | 0.0000 | 0.0 | 644892 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | 30 | 11 | high | 0 | 0 | 0 | 0 | 5 | 1 | 0 | 5 | 0 | 0 | 2950 | 870 | 870 | 468 | 1124 | 0.0000 | 0.0 | 724899 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | 31 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2930 | 892 | 892 | 548 | 904 | 0.0000 | 0.0 | 724785 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 29 | 24 | high | 0 | 0 | 0 | 0 | 6 | 2 | 0 | 6 | 0 | 0 | 6291 | 794 | 937 | 840 | 840 | 0.0000 | 7.0 | 641787 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 31 | 6 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3037 | 927 | 1014 | 500 | 1140 | 0.0821 | 0.0 | 635594 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2843 | 786 | 815 | 456 | 828 | 0.0000 | 0.0 | 631184 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2952 | 844 | 913 | 572 | 912 | 0.0000 | 0.0 | 632016 |
| logged-in-reader | mobile | reader-chat | /vi/readers | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2903 | 861 | 870 | 516 | 872 | 0.0000 | 0.0 | 633587 |
| logged-in-reader | mobile | reader-chat | /vi/chat | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2938 | 873 | 898 | 516 | 896 | 0.0000 | 0.0 | 631256 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3042 | 903 | 903 | 508 | 1168 | 0.0000 | 0.0 | 650832 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 30 | 8 | high | 0 | 0 | 0 | 0 | 1 | 0 | 1 | 1 | 0 | 0 | 3934 | 896 | 907 | 624 | 2016 | 0.0051 | 0.0 | 642765 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | 30 | 6 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2970 | 902 | 902 | 504 | 904 | 0.0000 | 0.0 | 643335 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2930 | 883 | 884 | 484 | 1164 | 0.0000 | 0.0 | 634419 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3112 | 789 | 816 | 476 | 868 | 0.0000 | 0.0 | 631095 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4079 | 1339 | 1541 | 832 | 832 | 0.0071 | 0.0 | 631471 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 31 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3937 | 1671 | 1671 | 928 | 1240 | 0.0330 | 0.0 | 634660 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3829 | 1281 | 1282 | 684 | 684 | 0.0000 | 0.0 | 632394 |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4095 | 1172 | 1350 | 876 | 876 | 0.0071 | 0.0 | 631773 |
| logged-in-reader | mobile | reading | /vi/reading/history | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4516 | 1368 | 1450 | 900 | 900 | 0.0071 | 0.0 | 633035 |
| logged-in-reader | mobile | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3627 | 1347 | 1347 | 1184 | 1512 | 0.0032 | 0.0 | 525694 |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3819 | 1225 | 1240 | 1284 | 1284 | 0.0032 | 0.0 | 525528 |
| logged-in-reader | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3022 | 814 | 888 | 824 | 1172 | 0.0032 | 0.0 | 525771 |
| logged-in-reader | mobile | reading | /vi/reading/session/80e076ca-dfff-469d-804e-1d85cdb0676d | 31 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3083 | 670 | 945 | 492 | 840 | 0.0000 | 0.0 | 681374 |
| logged-in-reader | mobile | reading | /vi/reading/session/cba5eb9d-ff2a-41c5-8b76-9bb5bcdc0989 | 33 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3155 | 831 | 1086 | 508 | 860 | 0.0001 | 0.0 | 692831 |
| logged-in-reader | mobile | reading | /vi/reading/session/16a7bb79-f7ea-4f53-8dbc-c5250273cddc | 33 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3037 | 825 | 994 | 492 | 860 | 0.0000 | 0.0 | 692781 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2885 | 804 | 821 | 480 | 832 | 0.0000 | 0.0 | 630705 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2853 | 781 | 823 | 472 | 816 | 0.0000 | 0.0 | 630865 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2836 | 745 | 753 | 496 | 844 | 0.0000 | 0.0 | 630798 |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2843 | 803 | 816 | 540 | 912 | 0.0000 | 0.0 | 632500 |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2819 | 750 | 753 | 472 | 824 | 0.0000 | 0.0 | 632435 |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2847 | 805 | 815 | 464 | 832 | 0.0000 | 0.0 | 632826 |

## Collection Image Focus
| Scenario | Viewport | Route | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reopen Img Requests | 304 Cache Hits |
| --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | /vi/collection | 13 | 3 | 5 | 12 | 1 | 0 |
| logged-in-reader | desktop | /vi/collection | 6 | 4 | 0 | 6 | 0 | 0 |
| logged-in-admin | mobile | /vi/collection | 13 | 3 | 5 | 12 | 1 | 0 |
| logged-in-reader | mobile | /vi/collection | 6 | 2 | 0 | 6 | 0 | 0 |

## Suspicious Pages (>25 requests)
| Scenario | Viewport | Feature | Route | Request Count | Severity | API | Static | Third-party |
| --- | --- | --- | --- | ---: | --- | ---: | ---: | ---: |
| logged-out | desktop | auth-public | /vi | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-admin | desktop | reading | /vi/reading | 29 | high | 1 | 26 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | 31 | high | 1 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | 32 | high | 2 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | 31 | high | 2 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers | 29 | high | 1 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 29 | high | 1 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | 29 | high | 1 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin | 30 | high | 1 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/deposits | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/gamification | 32 | high | 0 | 30 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/promotions | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/readings | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/users | 31 | high | 0 | 29 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/275c6df1-5a67-445b-a9db-b1d5ba316b23 | 34 | high | 2 | 30 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/698106b6-6bdb-4988-93e6-87034cd28eda | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/1a03a485-ec7d-4912-9cde-416b5bae69f3 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-reader | desktop | reading | /vi/reading | 30 | high | 2 | 26 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | 32 | high | 2 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | 32 | high | 2 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | 31 | high | 2 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | 29 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/84843a3c-745f-4205-8056-030a40207dde | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/b27bf782-c302-4ae8-ab53-0f71b70a5c67 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/bfb8919e-c739-44f7-8934-63cd5dbeb9e4 | 34 | high | 2 | 30 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-out | mobile | auth-public | /vi | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-admin | mobile | reading | /vi/reading | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | 32 | high | 2 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | 32 | high | 2 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | 30 | high | 1 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | 29 | high | 1 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | 31 | high | 1 | 28 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 35 | high | 1 | 32 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 30 | high | 1 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | 29 | high | 1 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | auth-public | /vi/legal/tos | 27 | high | 1 | 23 | 0 |
| logged-in-admin | mobile | admin | /vi/admin | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/deposits | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/disputes | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/gamification | 32 | high | 0 | 30 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/promotions | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/reader-requests | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/readings | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/users | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/ad085b60-8b51-43a6-9018-4217d2c3914d | 33 | high | 2 | 29 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/09ef272e-4364-4d56-82ba-498ed1c6ead7 | 33 | high | 2 | 29 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/56c81cad-8f33-4a8b-aae2-ea3499e78a3a | 31 | high | 1 | 28 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 29 | high | 1 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 29 | high | 1 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 30 | high | 2 | 26 | 0 |
| logged-in-reader | mobile | auth-public | /vi | 34 | high | 4 | 27 | 0 |
| logged-in-reader | mobile | reading | /vi/reading | 29 | high | 1 | 26 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | 31 | high | 1 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | 31 | high | 1 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 31 | high | 2 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | 30 | high | 1 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 29 | high | 1 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 31 | high | 2 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | 29 | high | 1 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/history | 29 | high | 1 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/80e076ca-dfff-469d-804e-1d85cdb0676d | 31 | high | 1 | 28 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/cba5eb9d-ff2a-41c5-8b76-9bb5bcdc0989 | 33 | high | 2 | 29 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/16a7bb79-f7ea-4f53-8dbc-c5250273cddc | 33 | high | 2 | 29 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |

## High Slow Requests (>800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1552 | 329 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 1383 | 67 | static | https://www.tarotnow.xyz/_next/static/chunks/0kn_-kiqq.6et.js |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 1380 | 85 | static | https://www.tarotnow.xyz/_next/static/chunks/0ryoba0b~me9m.js |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 1360 | 85 | static | https://www.tarotnow.xyz/_next/static/chunks/0c4xf8mjx0gok.js |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 1324 | 103 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 1243 | 356 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-reader | desktop | reading | /vi/reading/history | GET | 200 | 1233 | 421 | html | https://www.tarotnow.xyz/vi/reading/history |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 1226 | 323 | html | https://www.tarotnow.xyz/vi/reading/history |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1193 | 582 | html | https://www.tarotnow.xyz/vi |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 1188 | 308 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 1152 | 68 | static | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 1148 | 102 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 1147 | 103 | static | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | GET | 200 | 1146 | 360 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 1130 | 193 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 1128 | 175 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 1126 | 85 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 1126 | 166 | static | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| logged-in-reader | mobile | auth-public | /vi/legal/tos | GET | 200 | 1123 | 311 | html | https://www.tarotnow.xyz/vi/legal/tos |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 1121 | 192 | static | https://www.tarotnow.xyz/_next/static/chunks/0xovby2vf96zv.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 1111 | 95 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 1105 | 130 | static | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 1094 | 175 | static | https://www.tarotnow.xyz/_next/static/chunks/0h38g3weqde1h.js |
| logged-out | mobile | auth-public | /vi | GET | 200 | 1087 | 557 | html | https://www.tarotnow.xyz/vi |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 1083 | 104 | static | https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 1076 | 82 | static | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | GET | 200 | 1070 | 102 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 1068 | 73 | static | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 1063 | 192 | static | https://www.tarotnow.xyz/_next/static/chunks/0d2b1s5.nlpim.js |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 1047 | 81 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | GET | 200 | 1043 | 310 | html | https://www.tarotnow.xyz/vi/reader/apply |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 1039 | 113 | static | https://www.tarotnow.xyz/_next/static/chunks/0h38g3weqde1h.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 1036 | 105 | static | https://www.tarotnow.xyz/_next/static/chunks/0xovby2vf96zv.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 1034 | 104 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | GET | 200 | 1031 | 78 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 1028 | 91 | static | https://www.tarotnow.xyz/_next/static/chunks/0eqce2yjfryre.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 1025 | 106 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 1022 | 68 | static | https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 1020 | 92 | static | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 1017 | 101 | static | https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| logged-in-admin | mobile | auth-public | /vi/legal/tos | GET | 200 | 1011 | 333 | html | https://www.tarotnow.xyz/vi/legal/tos |
| logged-in-reader | desktop | reading | /vi/reading/history | GET | 200 | 1005 | 98 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 1005 | 66 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 992 | 92 | static | https://www.tarotnow.xyz/_next/static/chunks/0d2b1s5.nlpim.js |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 985 | 192 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 979 | 105 | telemetry | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| logged-in-reader | desktop | reading | /vi/reading/history | GET | 200 | 973 | 98 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-reader | desktop | reading | /vi/reading/history | GET | 200 | 968 | 166 | static | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 961 | 201 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | desktop | reading | /vi/reading/history | GET | 200 | 959 | 146 | static | https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 944 | 124 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 931 | 201 | static | https://www.tarotnow.xyz/_next/static/chunks/0kn_-kiqq.6et.js |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 930 | 378 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 928 | 145 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | GET | 200 | 914 | 174 | static | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-reader | desktop | reading | /vi/reading/history | GET | 200 | 910 | 165 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 908 | 175 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | GET | 200 | 899 | 138 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 896 | 91 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-reader | desktop | reading | /vi/reading/history | GET | 200 | 892 | 101 | static | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-reader | desktop | reading | /vi/reading/history | GET | 200 | 887 | 74 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 886 | 66 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 883 | 393 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | GET | 200 | 881 | 179 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 879 | 202 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | GET | 200 | 874 | 156 | static | https://www.tarotnow.xyz/_next/static/chunks/0kn_-kiqq.6et.js |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 872 | 329 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-reader | desktop | reading | /vi/reading/history | GET | 200 | 862 | 107 | static | https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| logged-in-reader | desktop | reading | /vi/reading/history | GET | 200 | 862 | 103 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 862 | 202 | static | https://www.tarotnow.xyz/_next/static/chunks/0xovby2vf96zv.js |
| logged-in-admin | mobile | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 860 | 313 | html | https://www.tarotnow.xyz/vi/legal/ai-disclaimer |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | GET | 200 | 859 | 316 | html | https://www.tarotnow.xyz/vi/legal/privacy |
| logged-in-reader | desktop | reading | /vi/reading/history | GET | 200 | 855 | 130 | static | https://www.tarotnow.xyz/_next/static/chunks/0kn_-kiqq.6et.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 854 | 348 | static | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| logged-in-reader | mobile | auth-public | /vi/legal/tos | GET | 200 | 854 | 131 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-reader | desktop | reading | /vi/reading/history | GET | 200 | 853 | 131 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 853 | 194 | static | https://www.tarotnow.xyz/_next/static/chunks/0eqce2yjfryre.js |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | GET | 200 | 852 | 178 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-reader | desktop | reading | /vi/reading/history | GET | 200 | 848 | 165 | static | https://www.tarotnow.xyz/_next/static/chunks/0h38g3weqde1h.js |
| logged-in-reader | desktop | reading | /vi/reading/history | GET | 200 | 848 | 167 | static | https://www.tarotnow.xyz/_next/static/chunks/0xovby2vf96zv.js |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 800 | 334 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 798 | 320 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 798 | 325 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 798 | 128 | static | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-reader | mobile | auth-public | /vi/legal/tos | GET | 200 | 798 | 130 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 795 | 314 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 794 | 329 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-reader | mobile | reader-chat | /vi/readers | GET | 200 | 794 | 324 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | desktop | reading | /vi/reading | GET | 200 | 792 | 312 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 790 | 354 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | mobile | reading | /vi/reading/history | GET | 200 | 790 | 88 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | GET | 200 | 788 | 325 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 788 | 307 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | GET | 200 | 787 | 321 | html | https://www.tarotnow.xyz/vi/reader/apply |
| logged-in-admin | mobile | reading | /vi/reading/history | GET | 200 | 787 | 104 | static | https://www.tarotnow.xyz/_next/static/chunks/0kn_-kiqq.6et.js |
| logged-in-admin | desktop | admin | /vi/admin/disputes | GET | 200 | 785 | 328 | html | https://www.tarotnow.xyz/vi/admin/disputes |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 785 | 299 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-reader | desktop | reader-chat | /vi/readers | GET | 200 | 782 | 363 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 781 | 312 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | GET | 200 | 780 | 151 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 778 | 314 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-admin | mobile | admin | /vi/admin/gamification | GET | 200 | 778 | 307 | html | https://www.tarotnow.xyz/vi/admin/gamification |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 777 | 297 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 776 | 201 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 775 | 310 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 774 | 403 | html | https://www.tarotnow.xyz/vi/profile/reader |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | GET | 200 | 774 | 176 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | desktop | reader-chat | /vi/readers | GET | 200 | 773 | 309 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | GET | 200 | 773 | 312 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 773 | 309 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 773 | 81 | static | https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 772 | 293 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1200&q=75 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 772 | 312 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-reader | mobile | reader-chat | /vi/chat | GET | 200 | 772 | 324 | html | https://www.tarotnow.xyz/vi/chat |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 772 | 77 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | GET | 200 | 771 | 314 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-admin | mobile | reading | /vi/reading/history | GET | 200 | 771 | 79 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 769 | 299 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 769 | 201 | static | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | GET | 200 | 768 | 156 | static | https://www.tarotnow.xyz/_next/static/chunks/0eqce2yjfryre.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 767 | 352 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 767 | 310 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 767 | 310 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 767 | 316 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-admin | mobile | reader-chat | /vi/readers | GET | 200 | 766 | 298 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 763 | 299 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-admin | mobile | reading | /vi/reading/session/56c81cad-8f33-4a8b-aae2-ea3499e78a3a | GET | 200 | 762 | 325 | html | https://www.tarotnow.xyz/vi/reading/session/56c81cad-8f33-4a8b-aae2-ea3499e78a3a |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | GET | 200 | 761 | 346 | html | https://www.tarotnow.xyz/vi/admin/system-configs |
| logged-in-admin | desktop | admin | /vi/admin/readings | GET | 200 | 758 | 345 | html | https://www.tarotnow.xyz/vi/admin/readings |
| logged-in-admin | mobile | admin | /vi/admin/readings | GET | 200 | 758 | 329 | html | https://www.tarotnow.xyz/vi/admin/readings |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 755 | 309 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 753 | 319 | html | https://www.tarotnow.xyz/vi/profile/reader |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 753 | 318 | html | https://www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | GET | 200 | 752 | 311 | html | https://www.tarotnow.xyz/vi/admin/withdrawals |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | GET | 200 | 751 | 144 | telemetry | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| logged-in-admin | mobile | reading | /vi/reading/history | GET | 200 | 750 | 141 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | mobile | admin | /vi/admin/reader-requests | GET | 200 | 749 | 305 | html | https://www.tarotnow.xyz/vi/admin/reader-requests |
| logged-in-reader | mobile | auth-public | /vi/legal/tos | GET | 200 | 749 | 70 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | mobile | auth-public | /vi/legal/tos | GET | 200 | 749 | 88 | static | https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| logged-in-reader | mobile | reading | /vi/reading/session/16a7bb79-f7ea-4f53-8dbc-c5250273cddc | GET | 200 | 749 | 329 | html | https://www.tarotnow.xyz/vi/reading/session/16a7bb79-f7ea-4f53-8dbc-c5250273cddc |
| logged-in-admin | desktop | admin | /vi/admin/users | GET | 200 | 744 | 340 | html | https://www.tarotnow.xyz/vi/admin/users |
| logged-in-admin | desktop | admin | /vi/admin/gamification | GET | 200 | 742 | 322 | html | https://www.tarotnow.xyz/vi/admin/gamification |
| logged-in-reader | mobile | auth-public | /vi/legal/tos | GET | 200 | 742 | 81 | static | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-admin | mobile | reading | /vi/reading/session/ad085b60-8b51-43a6-9018-4217d2c3914d | GET | 200 | 733 | 306 | html | https://www.tarotnow.xyz/vi/reading/session/ad085b60-8b51-43a6-9018-4217d2c3914d |
| logged-in-admin | mobile | admin | /vi/admin/disputes | GET | 200 | 732 | 329 | html | https://www.tarotnow.xyz/vi/admin/disputes |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 732 | 94 | static | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | GET | 200 | 732 | 173 | static | https://www.tarotnow.xyz/_next/static/chunks/0xovby2vf96zv.js |
| logged-in-admin | mobile | reading | /vi/reading/session/09ef272e-4364-4d56-82ba-498ed1c6ead7 | GET | 200 | 731 | 305 | html | https://www.tarotnow.xyz/vi/reading/session/09ef272e-4364-4d56-82ba-498ed1c6ead7 |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | GET | 200 | 731 | 169 | static | https://www.tarotnow.xyz/_next/static/chunks/0d2b1s5.nlpim.js |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 730 | 330 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | GET | 200 | 729 | 372 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 729 | 169 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 729 | 312 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-reader | mobile | auth-public | /vi/legal/tos | GET | 200 | 728 | 87 | static | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 727 | 308 | html | https://www.tarotnow.xyz/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| logged-in-admin | mobile | reading | /vi/reading/history | GET | 200 | 726 | 103 | static | https://www.tarotnow.xyz/_next/static/chunks/0d2b1s5.nlpim.js |
| logged-in-reader | mobile | auth-public | /vi | GET | 200 | 724 | 429 | html | https://www.tarotnow.xyz/vi |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 724 | 313 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| logged-in-reader | desktop | reading | /vi/reading/session/bfb8919e-c739-44f7-8934-63cd5dbeb9e4 | GET | 200 | 723 | 311 | html | https://www.tarotnow.xyz/vi/reading/session/bfb8919e-c739-44f7-8934-63cd5dbeb9e4 |
| logged-in-admin | mobile | admin | /vi/admin | GET | 200 | 723 | 323 | html | https://www.tarotnow.xyz/vi/admin |

## Pending Requests
| Scenario | Viewport | Route | URL |
| --- | --- | --- | --- |
| - | - | - | - |

## Coverage Notes
| Scenario | Viewport | Note |
| --- | --- | --- |
| logged-out | desktop | origin-discovery:/sitemap.xml:routes-22 |
| logged-out | desktop | origin-discovery:/robots.txt:routes-22 |
| logged-out | desktop | dynamic-routes: skipped for logged-out scenario. |
| logged-out | desktop | scenario-filter:logged-out-protected-routes-skipped=30 |
| logged-in-admin | desktop | origin-discovery:/sitemap.xml:routes-22 |
| logged-in-admin | desktop | origin-discovery:/robots.txt:routes-22 |
| logged-in-admin | desktop | reading.init.daily_1: blocked (400). |
| logged-in-admin | desktop | reading.init.spread_3: created 275c6df1-5a67-445b-a9db-b1d5ba316b23. |
| logged-in-admin | desktop | reading.init.spread_5: created 698106b6-6bdb-4988-93e6-87034cd28eda. |
| logged-in-admin | desktop | reading.init.spread_10: created 1a03a485-ec7d-4912-9cde-416b5bae69f3. |
| logged-in-admin | desktop | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-admin | desktop | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-admin | desktop | reader-detail:ui-discovery-empty |
| logged-in-admin | desktop | chat-room-detail:ui-discovery-empty |
| logged-in-admin | desktop | reading-history-detail:ui-discovery-empty |
| logged-in-admin | desktop | community-posts:api-discovery-1 |
| logged-in-admin | desktop | community-post-detail:69db54fc297f66f734421a3c:stale-page-404 |
| logged-in-admin | desktop | scenario-filter:admin-auth-entry-routes-skipped=5 |
| logged-in-reader | desktop | origin-discovery:/sitemap.xml:routes-22 |
| logged-in-reader | desktop | origin-discovery:/robots.txt:routes-22 |
| logged-in-reader | desktop | reading.init.daily_1: blocked (400). |
| logged-in-reader | desktop | reading.init.spread_3: created 84843a3c-745f-4205-8056-030a40207dde. |
| logged-in-reader | desktop | reading.init.spread_5: created b27bf782-c302-4ae8-ab53-0f71b70a5c67. |
| logged-in-reader | desktop | reading.init.spread_10: created bfb8919e-c739-44f7-8934-63cd5dbeb9e4. |
| logged-in-reader | desktop | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-reader | desktop | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-reader | desktop | reader-detail:ui-discovery-empty |
| logged-in-reader | desktop | chat-room-detail:ui-discovery-empty |
| logged-in-reader | desktop | reading-history-detail:ui-discovery-empty |
| logged-in-reader | desktop | community-posts:api-discovery-1 |
| logged-in-reader | desktop | community-post-detail:69db54fc297f66f734421a3c:stale-page-404 |
| logged-in-reader | desktop | scenario-filter:reader-auth-entry-admin-routes-skipped=15 |
| logged-out | mobile | origin-discovery:/sitemap.xml:routes-22 |
| logged-out | mobile | origin-discovery:/robots.txt:routes-22 |
| logged-out | mobile | dynamic-routes: skipped for logged-out scenario. |
| logged-out | mobile | scenario-filter:logged-out-protected-routes-skipped=30 |
| logged-in-admin | mobile | origin-discovery:/sitemap.xml:routes-22 |
| logged-in-admin | mobile | origin-discovery:/robots.txt:routes-22 |
| logged-in-admin | mobile | reading.init.daily_1: blocked (400). |
| logged-in-admin | mobile | reading.init.spread_3: created ad085b60-8b51-43a6-9018-4217d2c3914d. |
| logged-in-admin | mobile | reading.init.spread_5: created 09ef272e-4364-4d56-82ba-498ed1c6ead7. |
| logged-in-admin | mobile | reading.init.spread_10: created 56c81cad-8f33-4a8b-aae2-ea3499e78a3a. |
| logged-in-admin | mobile | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-admin | mobile | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-admin | mobile | reader-detail:ui-discovery-empty |
| logged-in-admin | mobile | chat-room-detail:ui-discovery-empty |
| logged-in-admin | mobile | reading-history-detail:ui-discovery-empty |
| logged-in-admin | mobile | community-posts:api-discovery-1 |
| logged-in-admin | mobile | community-post-detail:69db54fc297f66f734421a3c:stale-page-404 |
| logged-in-admin | mobile | scenario-filter:admin-auth-entry-routes-skipped=5 |
| logged-in-reader | mobile | origin-discovery:/sitemap.xml:routes-22 |
| logged-in-reader | mobile | origin-discovery:/robots.txt:routes-22 |
| logged-in-reader | mobile | reading.init.daily_1: blocked (400). |
| logged-in-reader | mobile | reading.init.spread_3: created 80e076ca-dfff-469d-804e-1d85cdb0676d. |
| logged-in-reader | mobile | reading.init.spread_5: created cba5eb9d-ff2a-41c5-8b76-9bb5bcdc0989. |
| logged-in-reader | mobile | reading.init.spread_10: created 16a7bb79-f7ea-4f53-8dbc-c5250273cddc. |
| logged-in-reader | mobile | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-reader | mobile | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-reader | mobile | reader-detail:ui-discovery-empty |
| logged-in-reader | mobile | chat-room-detail:ui-discovery-empty |
| logged-in-reader | mobile | reading-history-detail:ui-discovery-empty |
| logged-in-reader | mobile | community-posts:api-discovery-1 |
| logged-in-reader | mobile | community-post-detail:69db54fc297f66f734421a3c:stale-page-404 |
| logged-in-reader | mobile | scenario-filter:reader-auth-entry-admin-routes-skipped=15 |

## Login Bootstrap Notes
### logged-in-admin / desktop
- Attempt 1: login bootstrap succeeded.

### logged-in-reader / desktop
- Attempt 1: login bootstrap succeeded.

### logged-in-admin / mobile
- Attempt 1: login bootstrap succeeded.

### logged-in-reader / mobile
- Attempt 1: login bootstrap succeeded.
