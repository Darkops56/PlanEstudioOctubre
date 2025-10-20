import Layout from "../components/Layout/Layout";


export default function Home() {
  

  return (
     <Layout>
      <div className="flex flex-col items-center justify-center h-full text-center px-6">
        <h1 className="text-5xl font-extrabold text-white mb-6 animate-fadeIn">
          Bienvenido a Boletería Virtual
        </h1>
        <p className="text-lg text-gray-300 mb-10 animate-fadeIn delay-300">
          Gestiona tus clientes, productos y eventos de forma rápida y sencilla
        </p>
        <div className="animate-bounce">
          <svg
            className="w-12 h-12 text-indigo-500 mx-auto"
            fill="none"
            stroke="currentColor"
            strokeWidth="2"
            viewBox="0 0 24 24"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path strokeLinecap="round" strokeLinejoin="round" d="M19 9l-7 7-7-7"></path>
          </svg>
        </div>
      </div>
    </Layout>
  );
}
