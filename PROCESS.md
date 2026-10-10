# What context, constraints, or instructions did you give your agent before it coded?

For the backend, I added skeleton files of what I wanted and had Copilot fill out the actual implementation for me. I then reviewed what it had written and edited the code to fit the format that I wanted. I followed the coding patterns that I have been exposed to in my professional career. 

I was unable to enable plan mode in Visual Studio but I would typically use plan mode to describe the initial requirements with the output files that I expect and have the agent build a plan. I would then review the plan and make some edits until the plan aligned with what I wanted. I ended up writing skeleton code as a first pass in this instance to help guide an agent without planning mode in the correct direction. I also only had the agent edit one file at a time to ensure it was as accurate as possible.

I am a little less experienced with front end work but I was able to use plan mode in VS Code. For the UI, I described the different pages that I wanted available and what the features should be on each page. Once the agent gave me the plan, I made a couple of edits and had the agent execute the plan. 


# What did you decide to delegate vs write yourself, and why?

I wrote the initial skeleton of the backend code to guide the agent in the correct direction. I also made some changes so the code would follow the practices that I am most familiar with. I chose to make these manual edits to make sure that I would be able to understand what the code is doing if I come back and reopen this project without seeing it for a while.


# What did the agent do wrong that you had to fix? How did you fix it?

The agent was fairly accurate in its work. Most of the changes I made were related to formatting and organization. There were a couple of initial bugs but I was able to prompt the agent into fixing them without having to do any manual work. An example is with the HuggingFaceService. The initial code did not parse the response correctly. I was able to prompt the agent to fix the issue by providing more detailed instructions.

For the frontend work, the agent wrote most of the code. I made a few UI changes and mostly acted as QA for this work.