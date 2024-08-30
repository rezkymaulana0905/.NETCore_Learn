import '../constants/constants.dart';

class PhotoForm extends StatelessWidget {
  const PhotoForm({super.key, this.photo});

  final Uint8List? photo;

  @override
  Widget build(BuildContext context) {
    return Builder(
      builder: (context) {
        final size = _getSize(context.orientation);
        return photo == null ? _buildNoPhotoForm(size) : _buildPhotoForm(size);
      },
    );
  }

  Size _getSize(Orientation orientation) {
    return switch (orientation) {
      Orientation.portrait => Size(baseWidth * 0.5, baseHeight * 0.25),
      Orientation.landscape => Size(baseWidth * 0.3, baseHeight * 0.15),
    };
  }

  Widget _buildPhotoForm(Size size) {
    return ClipRRect(
      borderRadius: BorderRadius.circular(15),
      child: Image.memory(photo!, height: size.height),
    );
  }

  Widget _buildNoPhotoForm(Size size) {
    return Container(
      width: size.width,
      height: size.height,
      decoration: BoxDecoration(
        borderRadius: BorderRadius.circular(15),
        boxShadow: const <BoxShadow>[
          BoxShadow(
            color: AppColor.shadow,
            blurRadius: 3,
            blurStyle: BlurStyle.outer,
          ),
        ],
      ),
      child: Icon(Icons.camera_alt, size: size.width * 0.5),
    );
  }
}
