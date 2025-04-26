export default function Footer() {
    return (
      <footer className="bg-green-900 text-white py-4 ">
        <div className="mx-auto text-center">
          <p>&copy; {new Date().getFullYear()} Portfolio App. All rights reserved.</p>
        </div>
      </footer>
    );
  }
  