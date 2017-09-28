module NCF.BPP.About
open WebSharper.UI.Next.Html
open WebSharper.UI.Next

let internal paragraphs : (string * Doc list) list =
    [
    (
        "The BPP initiative",
        [p 
            [text 
                """
                Business Production Portal is an example of state of the art web-technology portal for Northparkes production data.
                While still quite basic, it could be the first seed of a new business production information website for Northparkes.
                """
            ]
        ]
    )
    (
        "The Basics",
        [p 
            [text 
                """
                Production data originate from the "production floor", where a series of sensors capture process variables and convert them into numbers. These numbers are then
                processed by computers that automatically take decisions and act on the actuators, devices capable to change the process itself in order to attain production objectives.
                All this data is also stored in historical databases that can then be queried to calculate summaries and KPIs.
                """
            ]
        ]
    )
    (
        "Northparkes current situation",
        [p 
            [text 
                """
                Our business has several different applications for this data (ProcessMORe UG and OPD, PI, IBA, SCADA, SAP, LIMS, AutoMine, Excel). Each one of them is separate from the others,
                not only as data repository, but especially as front-end. If you want to see PI data, you need to open excel and use Data Link, or ProcessBook and trend them.
                For more in depth trending capabilities you need to look inside the SCADA application, but not everything is there as for the Winder data is in the IBA logger.
                Laboratory data can be seen in the LIMS user interface and some of it go to ProcessMORe but much do not. Extraction level data is in the Automine Sandvik application and most of it is not
                even visible to the majority of us.
                We have an equipment database which is not centralized, but repeated into different application: SAP has its own, and so have ProcessMORe, SCADA, PI. 
                These are far from being synchronized, which of course makes more difficult to get good information out of them.
                """
            ]
        ]
    )
    (
        "Solutions?",
        [p 
            [text 
                """
                Is it possible to have a unique and uniform application showing all or at least the majority of production information? Of course it is, but how?
                The only public report sent to all in Northparkes is the daily production report sent to us through email. There is also ProcessMORe giving reports,
                but it is an old product difficult to maintain or upgrade.
                """
            ]
         p 
            [text 
                """
                BPP is a "stone in the pond" for something that could become a common production portal. Let's see some possible scenarios where BPP could be advantageous.
                """
            ]
         ul 
            [
            li
                [b [text "Power BI: "]
                 text """
                    See Power BI reports, with an important advantage: the user doesn't need to have a Power BI account. 
                    Once the report creator has published a report, it is immediately visible to all in BPP.
                    """
                ]
            li
                [b [text "Data input: "]
                 text """
                    Reporting is a view function, but sometimes it is necessary to change or add data. While the objective is to automate as much as possible
                    , there are always cases where this cannot be done or is not wanted, and it is required that users
                    at all levels are able to insert new or adjust existing data. For example, downtimes are manually inserted, or concentrate values
                    are adjusted referring to field log collections. Power BI, like other reporting platforms, is designed to provide powerful data query 
                    and presentation features but not so for input. BPP could then contain pages and features to input data.
                    """
                ]
            li
                [b [text "Integration: "]
                 text """
                    To obtain a unique wharehouse for business data in northparkes would be very difficult. There are multiple data originators and collectors.
                    However, what would probably be more useful and doable is develop a unique operational business application that fetches data from the different
                    sources, applies business rules to merge/elaborate them and renders them in a uniform user interface. BPP could become such an application.
                    """
                ]
            ]
        ]
    )
    (
        "Wrap up",
        [p 
            [text 
                """
                Please have a go with BPP; at this stage is minimal and experimental, but it gives the idea. Further ideas and suggestions can be sent do Franco and Mitch
                (Contacts in the navigation bar).
                """
            ]
         p 
            [text 
                """
                Thank you.
                """
            ]
        ]
    )
    ]