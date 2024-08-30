import '../../../constants/constants.dart';
import '../home.dart';

class LogActivity extends GetView<HomeController> {
  const LogActivity({super.key});

  @override
  Widget build(BuildContext context) {
    return Column(
      children: <Widget>[
        Row(
          mainAxisAlignment: MainAxisAlignment.center,
          children: <Widget>[
            const SizedBox(width: 50),
            Text(
              "Log Activity",
              style: TextStyle(
                fontSize: baseWidth * 0.08,
                fontWeight: FontWeight.w600,
              ),
            ),
            IconButton(
              onPressed: () => controller.getLogActivity(),
              icon: Icon(
                Icons.refresh,
                color: Colors.black,
                size: baseWidth * 0.08,
              ),
            ),
          ],
        ),
        Expanded(
          child: Padding(
            padding: const EdgeInsets.all(20.0),
            child: Obx(
              () => Row(
                children: [
                  ActivityLog(
                    title: "PLAN",
                    themeColor: AppColor.shadow,
                    activities: controller.home.value.logActivityPlan.obs,
                    controller: controller,
                    sortOrder: controller.sortOrder,
                  ),
                  const VerticalDivider(color: Colors.black, thickness: 5),
                  ActivityLog(
                    title: "IN",
                    themeColor: AppColor.enter,
                    activities: controller.home.value.logActivityIn.obs,
                    controller: controller,
                    sortOrder: controller.sortOrder,
                  ),
                  const VerticalDivider(color: Colors.black, thickness: 5),
                  ActivityLog(
                    title: "OUT",
                    themeColor: AppColor.leave,
                    activities: controller.home.value.logActivityOut.obs,
                    controller: controller,
                    sortOrder: controller.sortOrder,
                  ),
                ],
              ),
            ),
          ),
        ),
      ],
    );
  }
}

class ActivityLog extends StatelessWidget {
  ActivityLog({
    super.key,
    required this.controller,
    required this.title,
    required this.themeColor,
    required this.activities,
    required this.sortOrder,
  });

  final HomeController controller;
  final String title;
  final Color themeColor;
  final RxList<Activity> activities;
  final List<Map<String, dynamic>> sortOrder;
  final sortIndex = 0.obs;

  @override
  Widget build(BuildContext context) {
    return Expanded(
      child: Column(
        children: <Widget>[
          _buildHeader(),
          Obx(
            () => Expanded(
              child: ListView.builder(
                padding: const EdgeInsets.symmetric(vertical: 10),
                itemCount: activities.length,
                itemBuilder: (context, index) {
                  return ActivityCard(
                    activity: activities[index],
                    borderColor:
                        controller.getTypeColor(activities[index].detail),
                  );
                },
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildHeader() {
    return Container(
      decoration: BoxDecoration(
        borderRadius: BorderRadius.circular(10),
        color: themeColor,
      ),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceEvenly,
        children: <Widget>[
          Text(
            title,
            style: TextStyle(
              color: title == "PLAN" ? Colors.black : Colors.white,
              fontSize: baseWidth * 0.05,
              fontWeight: FontWeight.w600,
            ),
          ),
          Obx(() {
            return IconButton(
              onPressed: () {
                sortIndex.value =
                    controller.changeSortOrder(activities, sortIndex.value);
              },
              icon: Icon(
                sortOrder[sortIndex.value]['icon'],
                color: title == "PLAN" ? Colors.black : Colors.white,
              ),
            );
          }),
        ],
      ),
    );
  }
}

class ActivityCard extends StatelessWidget {
  const ActivityCard({
    super.key,
    required this.activity,
    required this.borderColor,
  });

  final Activity activity;
  final Color borderColor;

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 10.0, horizontal: 20),
      child: Container(
        decoration: BoxDecoration(
          borderRadius: BorderRadius.circular(10),
          border: Border.all(color: borderColor, width: 2.5),
        ),
        child: Column(
          children: <Widget>[
            if (activity.type.contains("-"))
              Column(
                children: [_buildActivityInfo(activity.type), const Divider()],
              ),
            _buildActivityInfo(activity.name),
            _buildActivityInfo(activity.date),
            _buildActivityInfo(activity.time),
          ],
        ),
      ),
    );
  }

  Widget _buildActivityInfo(String text) {
    return Padding(
      padding: const EdgeInsets.all(5.0),
      child: Text(
        text,
        textAlign: TextAlign.center,
        style: TextStyle(fontSize: baseWidth * 0.02),
      ),
    );
  }
}
